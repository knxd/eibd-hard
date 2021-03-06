/*
    EIBD eib bus access and management daemon
    Copyright (C) 2005-2011 Martin Koegler <mkoegler@auto.tuwien.ac.at>

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <argp.h>
#include "addrtab.h"
#include "lowlevelconf.h"
#include "ip/ipv4net.h"

/** aborts program with a printf like message */
void
die (const char *msg, ...)
{
  va_list ap;
  va_start (ap, msg);
  vprintf (msg, ap);
  printf ("\n");
  va_end (ap);

  exit (1);
}

/** structure to store low level backends */
struct urldef
{
  /** URL-prefix */
  const char *prefix;
  /** factory function */
  LowLevel_Create_Func Create;
  /** cleanup function */
  void (*Cleanup) ();
};

/** list of URLs */
struct urldef URLs[] = {
#undef L2_NAME
#define L2_NAME(a) { a##_PREFIX, a##_CREATE, a##_CLEANUP },
#include "lowlevelcreate.h"
  {0, 0, 0}
};

void (*Cleanup) ();

/** determines the right backend for the url and creates it */
LowLevelDriverInterface *
Create (const char *url,
    int flags,
    Logs * t,
    int inquemaxlen, int outquemaxlen,
    int maxpacketsoutpersecond)
{
  unsigned int p = 0;
  struct urldef *u = URLs;
  while (url[p] && url[p] != ':')
    p++;
  if (url[p] != ':')
    die ("not a valid url");
  while (u->prefix)
    {
      if (strlen (u->prefix) == p && !memcmp (u->prefix, url, p))
	{
	  Cleanup = u->Cleanup;
	  return u->Create (url + p + 1, t, flags, inquemaxlen, outquemaxlen,
	      maxpacketsoutpersecond);
	}
      u++;
    }
  die ("url not supported");
  return 0;
}

/** version */
const char *argp_program_version = "bcuaddrtab " VERSION;
/** documentation */
static char doc[] =
  "bcuaddrtab -- read BCU address table size (or write it with -w)\n"
  "(C) 2005-2011 Martin Koegler <mkoegler@auto.tuwien.ac.at>\n"
  "supported URLs are:\n"
#undef L2_NAME
#define L2_NAME(a) a##_URL
#include "lowlevelcreate.h"
  "\n"
#undef L2_NAME
#define L2_NAME(a) a##_DOC
#include "lowlevelcreate.h"
  "\n";

/** structure to store the arguments */
struct arguments
{
  /** trace level */
  int tracelevel;
  /** length to write */
  int newlength;
  /** timeout to wait */
  int timeout;
};
/** storage for the arguments*/
struct arguments arg;

/** documentation for arguments*/
static char args_doc[] = "URL";

/** option list */
static struct argp_option options[] = {

  {"trace", 't', "LEVEL", 0, "set trace level"},
  {"write", 'w', "SIZE", 0, "value to write"},
  {"timeout", 'T', "SECONDS", 0, "timelimit for the operation"},
  {0}
};


/** parses and stores an option */
static error_t
parse_opt (int key, char *arg, struct argp_state *state)
{
  struct arguments *arguments = (struct arguments *) state->input;
  switch (key)
    {
    case 't':
      arguments->tracelevel = (arg ? atoi (arg) : 0);
      break;
    case 'T':
      arguments->timeout = (arg ? atoi (arg) : 0);
      break;
    case 'w':
      arguments->newlength = (arg ? atoi (arg) : 0);
      break;
    default:
      return ARGP_ERR_UNKNOWN;
    }
  return 0;
}


/** information for the argument parser*/
static struct argp argp = { options, parse_opt, args_doc, doc };


int
main (int ac, char *ag[])
{
  int index;
  LowLevelDriverInterface *iface = 0;
  memset (&arg, 0, sizeof (arg));
  arg.newlength = -1;

  argp_parse (&argp, ac, ag, 0, &index, &arg);
  if (index > ac - 1)
    die ("url expected");
  if (index < ac - 1)
    die ("unexpected parameter");

  signal (SIGPIPE, SIG_IGN);
  pth_init ();

  Logs t;
  // printf ("tracing at: %d\n", arg.tracelevel);
  t.setTraceLevel (arg.tracelevel);
  IPv4NetList emptylist;
  iface = Create (ag[index], 0, &t, 0,0,0);
  if (!iface)
    die ("initialisation failed (no interface)");

  // pth_ctrl(PTH_CTRL_DUMPSTATE, stdout);
  pth_yield(NULL); // time for e'one to get the threads up
  if (!iface->init ())
    die ("initialisation failed (on init)");

  // we have to read now Layer2 Interface. reading the underlying interface
  // otherwise we'll compete for packets, architecture of the daemon was never
  // meant to be shortcuted like it was originally and hence all the problems
  // with timings & whatever not

  // wait until interface up and ready
  while (iface->Connection_Lost())
    pth_sleep(1);

  pth_event_t e = pth_event (PTH_EVENT_RTIME, pth_time (arg.timeout, 0));

  uchar res = arg.newlength;
  if (arg.newlength == -1)
    {
      if (!iface->readAddrTabSize (e,res))
	die ("read failed");
      if (pth_event_status(e) == PTH_STATUS_OCCURRED )
        die("timeout");

      printf ("Size: %d\n", res);
    }
  else if (arg.newlength >= 0 && arg.newlength <= 0xff)
    {
      if (!iface->writeAddrTabSize (e,res,true))
	die ("write failed");
      if (pth_event_status(e) == PTH_STATUS_OCCURRED )
        die("timeout");

      printf ("Size %d written\n", res);
    }
  else
    die ("invalid value %d to write", arg.newlength);

  delete iface;
  if (Cleanup)
    Cleanup ();

  pth_exit (0);
  return 0;
}
