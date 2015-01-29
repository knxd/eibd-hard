/*
    EIBD client library
    Copyright (C) 2005-2011 Martin Koegler <mkoegler@auto.tuwien.ac.at>

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    In addition to the permissions in the GNU General Public License, 
    you may link the compiled version of this file into combinations
    with other programs, and distribute those combinations without any 
    restriction coming from the use of this file. (The General Public 
    License restrictions do apply in other respects; for example, they 
    cover modification of the file, and distribution when not linked into 
    a combine executable.)

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Net;
using System.Net.Sockets;
namespace tuwien.auto.eibclient
{
  public class EIBBuffer
  {
    public byte[] data;
    public EIBBuffer (byte[]d)
    {
      data = d;
    }
    public EIBBuffer ()
    {
      data = null;
    }
  }
  public class EIBAddr
  {
    public ushort addr;
    public EIBAddr ()
    {
      addr = 0;
    }
    public EIBAddr (ushort val)
    {
      addr = val;
    }
  }
  public class UInt8
  {
    public byte data;
    public UInt8 ()
    {
      data = 0;
    }
    public UInt8 (byte value)
    {
      data = value;
    }
  }
  public class Int16
  {
    public short data;
    public Int16 ()
    {
      data = 0;
    }
    public Int16 (short value)
    {
      data = value;
    }
  }
  public class UInt16
  {
    public ushort data;
    public UInt16 ()
    {
      data = 0;
    }
    public UInt16 (ushort value)
    {
      data = value;
    }
  }
  public class UInt32
  {
    public uint data;
    public UInt32 ()
    {
      data = 0;
    }
    public UInt32 (ushort value)
    {
      data = value;
    }
  }

  public class EIBConnection
  {
    public const int EINVAL = 1;
    public const int ECONNRESET = 2;
    public const int EBUSY = 3;
    public const int EADDRINUSE = 4;
    public const int ETIMEDOUT = 5;
    public const int EADDRNOTAVAIL = 6;
    public const int EIO = 7;
    public const int EPERM = 8;
    public const int ENOENT = 9;
    public const int ENODEV = 10;
    private int errno = 0;
    public int getLastError ()
    {
      return errno;
    }
    public const int IMG_UNKNOWN_ERROR = 0;
    public const int IMG_UNRECOG_FORMAT = 1;
    public const int IMG_INVALID_FORMAT = 2;
    public const int IMG_NO_BCUTYPE = 3;
    public const int IMG_UNKNOWN_BCUTYPE = 4;
    public const int IMG_NO_CODE = 5;
    public const int IMG_NO_SIZE = 6;
    public const int IMG_LODATA_OVERFLOW = 7;
    public const int IMG_HIDATA_OVERFLOW = 8;
    public const int IMG_TEXT_OVERFLOW = 9;
    public const int IMG_NO_ADDRESS = 10;
    public const int IMG_WRONG_SIZE = 11;
    public const int IMG_IMAGE_LOADABLE = 12;
    public const int IMG_NO_DEVICE_CONNECTION = 13;
    public const int IMG_MASK_READ_FAILED = 14;
    public const int IMG_WRONG_MASK_VERSION = 15;
    public const int IMG_CLEAR_ERROR = 16;
    public const int IMG_RESET_ADDR_TAB = 17;
    public const int IMG_LOAD_HEADER = 18;
    public const int IMG_LOAD_MAIN = 19;
    public const int IMG_ZERO_RAM = 20;
    public const int IMG_FINALIZE_ADDR_TAB = 21;
    public const int IMG_PREPARE_RUN = 22;
    public const int IMG_RESTART = 23;
    public const int IMG_LOADED = 24;
    public const int IMG_NO_START = 25;
    public const int IMG_WRONG_ADDRTAB = 26;
    public const int IMG_ADDRTAB_OVERFLOW = 27;
    public const int IMG_OVERLAP_ASSOCTAB = 28;
    public const int IMG_OVERLAP_TEXT = 29;
    public const int IMG_NEGATIV_TEXT_SIZE = 30;
    public const int IMG_OVERLAP_PARAM = 31;
    public const int IMG_OVERLAP_EEPROM = 32;
    public const int IMG_OBJTAB_OVERFLOW = 33;
    public const int IMG_WRONG_LOADCTL = 34;
    public const int IMG_UNLOAD_ADDR = 35;
    public const int IMG_UNLOAD_ASSOC = 36;
    public const int IMG_UNLOAD_PROG = 37;
    public const int IMG_LOAD_ADDR = 38;
    public const int IMG_WRITE_ADDR = 39;
    public const int IMG_SET_ADDR = 40;
    public const int IMG_FINISH_ADDR = 41;
    public const int IMG_LOAD_ASSOC = 42;
    public const int IMG_WRITE_ASSOC = 43;
    public const int IMG_SET_ASSOC = 44;
    public const int IMG_FINISH_ASSOC = 45;
    public const int IMG_LOAD_PROG = 46;
    public const int IMG_ALLOC_LORAM = 47;
    public const int IMG_ALLOC_HIRAM = 48;
    public const int IMG_ALLOC_INIT = 49;
    public const int IMG_ALLOC_RO = 50;
    public const int IMG_ALLOC_EEPROM = 51;
    public const int IMG_ALLOC_PARAM = 52;
    public const int IMG_SET_PROG = 53;
    public const int IMG_SET_TASK_PTR = 54;
    public const int IMG_SET_OBJ = 55;
    public const int IMG_SET_TASK2 = 56;
    public const int IMG_FINISH_PROC = 57;
    public const int IMG_WRONG_CHECKLIM = 58;
    public const int IMG_INVALID_KEY = 59;
    public const int IMG_AUTHORIZATION_FAILED = 60;
    public const int IMG_KEY_WRITE = 61;
    private EIBBuffer buf;
    private Int16 ptr1;
    private UInt8 ptr2;
    private UInt8 ptr3;
    private UInt16 ptr4;
    private EIBAddr ptr5;
    private EIBAddr ptr6;
    private UInt32 ptr7;
    private int sendlen;
    private delegate int complete_t ();
    private complete_t complete = null;
    public int EIBComplete ()
    {
      if (complete == null)
	{
	  errno = EINVAL;
	  return -1;
	}
      return complete ();
    }
    private int readlen = 0;
    private byte[] head = null;
    private byte[] data = null;
    private Socket sock = null;
  public EIBConnection (String host):
    this (host, 6720)
    {
    }
    public EIBConnection (String host, int port)
    {
      IPAddress ip = Dns.GetHostEntry (host).AddressList[0];
      IPEndPoint endpoint = new IPEndPoint (ip, port);
      sock = new Socket (AddressFamily.InterNetwork,
			 SocketType.Stream, ProtocolType.Tcp);
      sock.Connect (endpoint);
      sock.SetSocketOption (SocketOptionLevel.Tcp, SocketOptionName.NoDelay,
			    1);
    }
    protected int _EIB_SendRequest (byte[]data)
    {
      if (sock == null)
	throw new Exception ("connection closed");
      byte[]len = new byte[2];
      if (data.Length > 0xffff || data.Length < 2)
	{
	  errno = EINVAL;
	  return -1;
	}
      len[0] = (byte) ((data.Length >> 8) & 0xff);
      len[1] = (byte) ((data.Length) & 0xff);
      sock.Send (len);
      sock.Send (data);
      return 0;
    }
    protected int _EIB_CheckRequest (bool block)
    {
      int res;
      if (sock == null)
	throw new Exception ("connection closed");
      if (readlen == 0)
	head = new byte[2];
      if (!block && sock.Available < 1)
	return 0;
      if (readlen < 2)
	{
	  res = sock.Receive (head, readlen, 2 - readlen, SocketFlags.None);
	  if (res <= 0)
	    {
	      throw new Exception ("connection closed");
	    }
	  readlen += res;
	  if (readlen < 2)
	    return 0;
	  res =
	    (((((int) (head[0])) & 0xff) << 8) |
	     ((((int) (head[1])) & 0xff)));
	  data = new byte[res];
	}
      if (!block && sock.Available < 1)
	return 0;
      res = sock.Receive (data, readlen - 2, data.Length + 2 - readlen,
			  SocketFlags.None);
      if (res <= 0)
	{
	  throw new Exception ("connection closed");
	}
      readlen += res;
      return 0;
    }
    protected int _EIB_GetRequest ()
    {
      do
	{
	  if (_EIB_CheckRequest (true) == -1)
	    return -1;
	}
      while (readlen < 2 || (readlen >= 2 && readlen < data.Length + 2));
      readlen = 0;
      return 0;
    }
    public int EIB_Poll_Complete ()
    {
      if (_EIB_CheckRequest (false) == -1)
	return -1;
      if (readlen < 2 || (readlen >= 2 && readlen < data.Length + 2))
	return 0;
      return 1;
    }
    public int EIBClose ()
    {
      if (sock == null)
	throw new Exception ("connection closed");
      try
      {
	sock.Shutdown (SocketShutdown.Both);
	sock.Close ();
      }
      finally
      {
	sock = null;
      }
      return 0;
    }
    public int EIBClose_sync ()
    {
      try
      {
	EIBReset ();
      }
      catch (Exception e)
      {
      }
      return EIBClose ();
    }


    private int EIBGetAPDU_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0025 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIBGetAPDU_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      complete = new complete_t (EIBGetAPDU_complete);
      return 0;
    }
    public int EIBGetAPDU (EIBBuffer buf)
    {
      if (EIBGetAPDU_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBGetAPDU_Src_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0025 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr5 != null)
	ptr5.addr =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      i = data.Length - 4;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 4];
      return i;
    }
    public int EIBGetAPDU_Src_async (EIBBuffer buf, EIBAddr src)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr5 = src;
      complete = new complete_t (EIBGetAPDU_Src_complete);
      return 0;
    }
    public int EIBGetAPDU_Src (EIBBuffer buf, EIBAddr src)
    {
      if (EIBGetAPDU_Src_async (buf, src) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBGetBusmonitorPacket_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0014 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIBGetBusmonitorPacket_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      complete = new complete_t (EIBGetBusmonitorPacket_complete);
      return 0;
    }
    public int EIBGetBusmonitorPacket (EIBBuffer buf)
    {
      if (EIBGetBusmonitorPacket_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBGetGroup_Src_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0027 || data.Length < 6)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr5 != null)
	ptr5.addr =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      if (ptr6 != null)
	ptr6.addr =
	  (ushort) (((((int) (data[4])) & 0xff) << 8) |
		    ((((int) (data[4 + 1])) & 0xff)));
      i = data.Length - 6;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 6];
      return i;
    }
    public int EIBGetGroup_Src_async (EIBBuffer buf, EIBAddr src,
				      EIBAddr dest)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr5 = src;
      this.ptr6 = dest;
      complete = new complete_t (EIBGetGroup_Src_complete);
      return 0;
    }
    public int EIBGetGroup_Src (EIBBuffer buf, EIBAddr src, EIBAddr dest)
    {
      if (EIBGetGroup_Src_async (buf, src, dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBGetTPDU_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0025 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr5 != null)
	ptr5.addr =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      i = data.Length - 4;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 4];
      return i;
    }
    public int EIBGetTPDU_async (EIBBuffer buf, EIBAddr src)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr5 = src;
      complete = new complete_t (EIBGetTPDU_complete);
      return 0;
    }
    public int EIBGetTPDU (EIBBuffer buf, EIBAddr src)
    {
      if (EIBGetTPDU_async (buf, src) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Clear_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0072 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_Cache_Clear_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0072 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0072) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Clear_complete);
      return 0;
    }
    public int EIB_Cache_Clear ()
    {
      if (EIB_Cache_Clear_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Disable_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0071 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_Cache_Disable_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0071 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0071) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Disable_complete);
      return 0;
    }
    public int EIB_Cache_Disable ()
    {
      if (EIB_Cache_Disable_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Enable_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0001)
	{
	  errno = EBUSY;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0070 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_Cache_Enable_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0070 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0070) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Enable_complete);
      return 0;
    }
    public int EIB_Cache_Enable ()
    {
      if (EIB_Cache_Enable_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Read_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0075 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if ((((((int) (data[4])) & 0xff) << 8) |
	   ((((int) (data[4 + 1])) & 0xff))) == 0)
	{
	  errno = ENODEV;
	  return -1;
	}
      if (data.Length <= 6)
	{
	  errno = ENOENT;
	  return -1;
	}
      if (ptr5 != null)
	ptr5.addr =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      i = data.Length - 6;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 6];
      return i;
    }
    public int EIB_Cache_Read_async (ushort dst, EIBAddr src, EIBBuffer buf)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr5 = src;
      ibuf[2] = (byte) ((dst >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dst) & 0xff);
      ibuf[0] = (byte) ((0x0075 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0075) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Read_complete);
      return 0;
    }
    public int EIB_Cache_Read (ushort dst, EIBAddr src, EIBBuffer buf)
    {
      if (EIB_Cache_Read_async (dst, src, buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Read_Sync_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0074 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if ((((((int) (data[4])) & 0xff) << 8) |
	   ((((int) (data[4 + 1])) & 0xff))) == 0)
	{
	  errno = ENODEV;
	  return -1;
	}
      if (data.Length <= 6)
	{
	  errno = ENOENT;
	  return -1;
	}
      if (ptr5 != null)
	ptr5.addr =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      i = data.Length - 6;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 6];
      return i;
    }
    public int EIB_Cache_Read_Sync_async (ushort dst, EIBAddr src,
					  EIBBuffer buf, ushort age)
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr5 = src;
      ibuf[2] = (byte) ((dst >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dst) & 0xff);
      ibuf[4] = (byte) ((age >> 8) & 0xff);
      ibuf[4 + 1] = (byte) ((age) & 0xff);
      ibuf[0] = (byte) ((0x0074 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0074) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Read_Sync_complete);
      return 0;
    }
    public int EIB_Cache_Read_Sync (ushort dst, EIBAddr src, EIBBuffer buf,
				    ushort age)
    {
      if (EIB_Cache_Read_Sync_async (dst, src, buf, age) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_Remove_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0073 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_Cache_Remove_async (ushort dest)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0073 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0073) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_Remove_complete);
      return 0;
    }
    public int EIB_Cache_Remove (ushort dest)
    {
      if (EIB_Cache_Remove_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_Cache_LastUpdates_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0076 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr4 != null)
	ptr4.data =
	  (ushort) (((((int) (data[2])) & 0xff) << 8) |
		    ((((int) (data[2 + 1])) & 0xff)));
      i = data.Length - 4;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 4];
      return i;
    }
    public int EIB_Cache_LastUpdates_async (ushort start, byte timeout,
					    EIBBuffer buf, UInt16 ende)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      this.buf = buf;
      this.ptr4 = ende;
      ibuf[2] = (byte) ((start >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((start) & 0xff);
      ibuf[4] = (byte) ((timeout) & 0xff);
      ibuf[0] = (byte) ((0x0076 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0076) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_Cache_LastUpdates_complete);
      return 0;
    }
    public int EIB_Cache_LastUpdates (ushort start, byte timeout,
				      EIBBuffer buf, UInt16 ende)
    {
      if (EIB_Cache_LastUpdates_async (start, timeout, buf, ende) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_LoadImage_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0063 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((((int) (data[2])) & 0xff) << 8) |
	      ((((int) (data[2 + 1])) & 0xff)));
    }
    public int EIB_LoadImage_async (byte[]image)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      if (image.Length < 0)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + image.Length];
      sendlen = image.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < image.Length; i++)
	ibuf[i + head.Length] = image[i];
      ibuf[0] = (byte) ((0x0063 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0063) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_LoadImage_complete);
      return 0;
    }
    public int EIB_LoadImage (byte[]image)
    {
      if (EIB_LoadImage_async (image) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Authorize_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0057 || data.Length < 3)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((int) (data[2])) & 0xff);
    }
    public int EIB_MC_Authorize_async (byte[]key)
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      if (key.Length != 4)
	throw new IndexOutOfRangeException ("key is not 4 bytes long");
      for (int i = 0; i < 4; i++)
	ibuf[2 + i] = key[i];
      ibuf[0] = (byte) ((0x0057 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0057) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Authorize_complete);
      return 0;
    }
    public int EIB_MC_Authorize (byte[]key)
    {
      if (EIB_MC_Authorize_async (key) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Connect_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0050 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Connect_async (ushort dest)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0050 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0050) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Connect_complete);
      return 0;
    }
    public int EIB_MC_Connect (ushort dest)
    {
      if (EIB_MC_Connect_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Individual_Open_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0049 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Individual_Open_async (ushort dest)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0049 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0049) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Individual_Open_complete);
      return 0;
    }
    public int EIB_MC_Individual_Open (ushort dest)
    {
      if (EIB_MC_Individual_Open_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_GetMaskVersion_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0059 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((((int) (data[2])) & 0xff) << 8) |
	      ((((int) (data[2 + 1])) & 0xff)));
    }
    public int EIB_MC_GetMaskVersion_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0059 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0059) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_GetMaskVersion_complete);
      return 0;
    }
    public int EIB_MC_GetMaskVersion ()
    {
      if (EIB_MC_GetMaskVersion_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_GetPEIType_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0055 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((((int) (data[2])) & 0xff) << 8) |
	      ((((int) (data[2 + 1])) & 0xff)));
    }
    public int EIB_MC_GetPEIType_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0055 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0055) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_GetPEIType_complete);
      return 0;
    }
    public int EIB_MC_GetPEIType ()
    {
      if (EIB_MC_GetPEIType_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Progmode_Off_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0060 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Progmode_Off_async ()
    {
      byte[]head = new byte[3];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((0) & 0xff);
      ibuf[0] = (byte) ((0x0060 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0060) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Progmode_Off_complete);
      return 0;
    }
    public int EIB_MC_Progmode_Off ()
    {
      if (EIB_MC_Progmode_Off_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Progmode_On_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0060 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Progmode_On_async ()
    {
      byte[]head = new byte[3];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((1) & 0xff);
      ibuf[0] = (byte) ((0x0060 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0060) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Progmode_On_complete);
      return 0;
    }
    public int EIB_MC_Progmode_On ()
    {
      if (EIB_MC_Progmode_On_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Progmode_Status_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0060 || data.Length < 3)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((int) (data[2])) & 0xff);
    }
    public int EIB_MC_Progmode_Status_async ()
    {
      byte[]head = new byte[3];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((3) & 0xff);
      ibuf[0] = (byte) ((0x0060 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0060) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Progmode_Status_complete);
      return 0;
    }
    public int EIB_MC_Progmode_Status ()
    {
      if (EIB_MC_Progmode_Status_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Progmode_Toggle_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0060 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Progmode_Toggle_async ()
    {
      byte[]head = new byte[3];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((2) & 0xff);
      ibuf[0] = (byte) ((0x0060 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0060) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Progmode_Toggle_complete);
      return 0;
    }
    public int EIB_MC_Progmode_Toggle ()
    {
      if (EIB_MC_Progmode_Toggle_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_PropertyDesc_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0061 || data.Length < 6)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr2 != null)
	ptr2.data = data[2];
      if (ptr4 != null)
	ptr4.data =
	  (ushort) (((((int) (data[3])) & 0xff) << 8) |
		    ((((int) (data[3 + 1])) & 0xff)));
      if (ptr3 != null)
	ptr3.data = data[5];
      return 0;
    }
    public int EIB_MC_PropertyDesc_async (byte obj, byte propertyno,
					  UInt8 proptype,
					  UInt16 max_nr_of_elem, UInt8 access)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      this.ptr2 = proptype;
      this.ptr4 = max_nr_of_elem;
      this.ptr3 = access;
      ibuf[2] = (byte) ((obj) & 0xff);
      ibuf[3] = (byte) ((propertyno) & 0xff);
      ibuf[0] = (byte) ((0x0061 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0061) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_PropertyDesc_complete);
      return 0;
    }
    public int EIB_MC_PropertyDesc (byte obj, byte propertyno, UInt8 proptype,
				    UInt16 max_nr_of_elem, UInt8 access)
    {
      if (EIB_MC_PropertyDesc_async
	  (obj, propertyno, proptype, max_nr_of_elem, access) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_PropertyRead_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0053 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_MC_PropertyRead_async (byte obj, byte propertyno,
					  ushort start, byte nr_of_elem,
					  EIBBuffer buf)
    {
      byte[]head = new byte[7];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[2] = (byte) ((obj) & 0xff);
      ibuf[3] = (byte) ((propertyno) & 0xff);
      ibuf[4] = (byte) ((start >> 8) & 0xff);
      ibuf[4 + 1] = (byte) ((start) & 0xff);
      ibuf[6] = (byte) ((nr_of_elem) & 0xff);
      ibuf[0] = (byte) ((0x0053 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0053) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_PropertyRead_complete);
      return 0;
    }
    public int EIB_MC_PropertyRead (byte obj, byte propertyno, ushort start,
				    byte nr_of_elem, EIBBuffer buf)
    {
      if (EIB_MC_PropertyRead_async (obj, propertyno, start, nr_of_elem, buf)
	  == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_PropertyScan_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0062 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_MC_PropertyScan_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[0] = (byte) ((0x0062 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0062) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_PropertyScan_complete);
      return 0;
    }
    public int EIB_MC_PropertyScan (EIBBuffer buf)
    {
      if (EIB_MC_PropertyScan_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_PropertyWrite_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0054 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_MC_PropertyWrite_async (byte obj, byte propertyno,
					   ushort start, byte nr_of_elem,
					   byte[]buf, EIBBuffer res)
    {
      byte[]head = new byte[7];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((obj) & 0xff);
      ibuf[3] = (byte) ((propertyno) & 0xff);
      ibuf[4] = (byte) ((start >> 8) & 0xff);
      ibuf[4 + 1] = (byte) ((start) & 0xff);
      ibuf[6] = (byte) ((nr_of_elem) & 0xff);
      if (buf.Length < 0)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + buf.Length];
      sendlen = buf.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < buf.Length; i++)
	ibuf[i + head.Length] = buf[i];
      this.buf = res;
      ibuf[0] = (byte) ((0x0054 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0054) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_PropertyWrite_complete);
      return 0;
    }
    public int EIB_MC_PropertyWrite (byte obj, byte propertyno, ushort start,
				     byte nr_of_elem, byte[]buf,
				     EIBBuffer res)
    {
      if (EIB_MC_PropertyWrite_async
	  (obj, propertyno, start, nr_of_elem, buf, res) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_ReadADC_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0056 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      if (ptr1 != null)
	ptr1.data =
	  (short) (((((int) (data[2])) & 0xff) << 8) |
		   ((((int) (data[2 + 1])) & 0xff)));
      return 0;
    }
    public int EIB_MC_ReadADC_async (byte channel, byte count, Int16 val)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      this.ptr1 = val;
      ibuf[2] = (byte) ((channel) & 0xff);
      ibuf[3] = (byte) ((count) & 0xff);
      ibuf[0] = (byte) ((0x0056 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0056) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_ReadADC_complete);
      return 0;
    }
    public int EIB_MC_ReadADC (byte channel, byte count, Int16 val)
    {
      if (EIB_MC_ReadADC_async (channel, count, val) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Read_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0051 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_MC_Read_async (ushort addr, int buf_len, EIBBuffer buf)
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[2] = (byte) ((addr >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((addr) & 0xff);
      ibuf[4] = (byte) (((buf_len) >> 8) & 0xff);
      ibuf[4 + 1] = (byte) (((buf_len)) & 0xff);
      ibuf[0] = (byte) ((0x0051 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0051) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Read_complete);
      return 0;
    }
    public int EIB_MC_Read (ushort addr, int buf_len, EIBBuffer buf)
    {
      if (EIB_MC_Read_async (addr, buf_len, buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Restart_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x005a || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_Restart_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x005a >> 8) & 0xff);
      ibuf[1] = (byte) ((0x005a) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Restart_complete);
      return 0;
    }
    public int EIB_MC_Restart ()
    {
      if (EIB_MC_Restart_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_SetKey_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0002)
	{
	  errno = EPERM;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0058 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_MC_SetKey_async (byte[]key, byte level)
    {
      byte[]head = new byte[7];
      byte[]ibuf = head;
      if (key.Length != 4)
	throw new IndexOutOfRangeException ("key is not 4 bytes long");
      for (int i = 0; i < 4; i++)
	ibuf[2 + i] = key[i];
      ibuf[6] = (byte) ((level) & 0xff);
      ibuf[0] = (byte) ((0x0058 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0058) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_SetKey_complete);
      return 0;
    }
    public int EIB_MC_SetKey (byte[]key, byte level)
    {
      if (EIB_MC_SetKey_async (key, level) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Write_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0044)
	{
	  errno = EIO;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0052 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return sendlen;
    }
    public int EIB_MC_Write_async (ushort addr, byte[]buf)
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((addr >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((addr) & 0xff);
      ibuf[4] = (byte) (((buf.Length) >> 8) & 0xff);
      ibuf[4 + 1] = (byte) (((buf.Length)) & 0xff);
      if (buf.Length < 0)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + buf.Length];
      sendlen = buf.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < buf.Length; i++)
	ibuf[i + head.Length] = buf[i];
      ibuf[0] = (byte) ((0x0052 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0052) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Write_complete);
      return 0;
    }
    public int EIB_MC_Write (ushort addr, byte[]buf)
    {
      if (EIB_MC_Write_async (addr, buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_MC_Write_Plain_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x005b || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return sendlen;
    }
    public int EIB_MC_Write_Plain_async (ushort addr, byte[]buf)
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((addr >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((addr) & 0xff);
      ibuf[4] = (byte) (((buf.Length) >> 8) & 0xff);
      ibuf[4 + 1] = (byte) (((buf.Length)) & 0xff);
      if (buf.Length < 0)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + buf.Length];
      sendlen = buf.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < buf.Length; i++)
	ibuf[i + head.Length] = buf[i];
      ibuf[0] = (byte) ((0x005b >> 8) & 0xff);
      ibuf[1] = (byte) ((0x005b) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_MC_Write_Plain_complete);
      return 0;
    }
    public int EIB_MC_Write_Plain (ushort addr, byte[]buf)
    {
      if (EIB_MC_Write_Plain_async (addr, buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_GetMaskVersion_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0031 || data.Length < 4)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((((int) (data[2])) & 0xff) << 8) |
	      ((((int) (data[2 + 1])) & 0xff)));
    }
    public int EIB_M_GetMaskVersion_async (ushort dest)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0031 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0031) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_GetMaskVersion_complete);
      return 0;
    }
    public int EIB_M_GetMaskVersion (ushort dest)
    {
      if (EIB_M_GetMaskVersion_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_Progmode_Off_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0030 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_M_Progmode_Off_async (ushort dest)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((0) & 0xff);
      ibuf[0] = (byte) ((0x0030 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0030) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_Progmode_Off_complete);
      return 0;
    }
    public int EIB_M_Progmode_Off (ushort dest)
    {
      if (EIB_M_Progmode_Off_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_Progmode_On_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0030 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_M_Progmode_On_async (ushort dest)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((1) & 0xff);
      ibuf[0] = (byte) ((0x0030 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0030) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_Progmode_On_complete);
      return 0;
    }
    public int EIB_M_Progmode_On (ushort dest)
    {
      if (EIB_M_Progmode_On_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_Progmode_Status_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0030 || data.Length < 3)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return (((int) (data[2])) & 0xff);
    }
    public int EIB_M_Progmode_Status_async (ushort dest)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((3) & 0xff);
      ibuf[0] = (byte) ((0x0030 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0030) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_Progmode_Status_complete);
      return 0;
    }
    public int EIB_M_Progmode_Status (ushort dest)
    {
      if (EIB_M_Progmode_Status_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_Progmode_Toggle_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0030 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_M_Progmode_Toggle_async (ushort dest)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((2) & 0xff);
      ibuf[0] = (byte) ((0x0030 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0030) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_Progmode_Toggle_complete);
      return 0;
    }
    public int EIB_M_Progmode_Toggle (ushort dest)
    {
      if (EIB_M_Progmode_Toggle_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_ReadIndividualAddresses_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0032 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_M_ReadIndividualAddresses_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[0] = (byte) ((0x0032 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0032) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_ReadIndividualAddresses_complete);
      return 0;
    }
    public int EIB_M_ReadIndividualAddresses (EIBBuffer buf)
    {
      if (EIB_M_ReadIndividualAddresses_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIB_M_WriteIndividualAddress_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0041)
	{
	  errno = EADDRINUSE;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0043)
	{
	  errno = ETIMEDOUT;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0042)
	{
	  errno = EADDRNOTAVAIL;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0040 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIB_M_WriteIndividualAddress_async (ushort dest)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0040 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0040) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_M_WriteIndividualAddress_complete);
      return 0;
    }
    public int EIB_M_WriteIndividualAddress (ushort dest)
    {
      if (EIB_M_WriteIndividualAddress_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenBusmonitor_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0001)
	{
	  errno = EBUSY;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0010 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenBusmonitor_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0010 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0010) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenBusmonitor_complete);
      return 0;
    }
    public int EIBOpenBusmonitor ()
    {
      if (EIBOpenBusmonitor_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenBusmonitorText_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0001)
	{
	  errno = EBUSY;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0011 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenBusmonitorText_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0011 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0011) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenBusmonitorText_complete);
      return 0;
    }
    public int EIBOpenBusmonitorText ()
    {
      if (EIBOpenBusmonitorText_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpen_GroupSocket_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0026 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpen_GroupSocket_async (bool write_only)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[4] = (byte) ((write_only) ? 0xff : 0);
      ibuf[0] = (byte) ((0x0026 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0026) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpen_GroupSocket_complete);
      return 0;
    }
    public int EIBOpen_GroupSocket (bool write_only)
    {
      if (EIBOpen_GroupSocket_async (write_only) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenT_Broadcast_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0023 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenT_Broadcast_async (bool write_only)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[4] = (byte) ((write_only) ? 0xff : 0);
      ibuf[0] = (byte) ((0x0023 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0023) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenT_Broadcast_complete);
      return 0;
    }
    public int EIBOpenT_Broadcast (bool write_only)
    {
      if (EIBOpenT_Broadcast_async (write_only) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenT_Connection_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0020 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenT_Connection_async (ushort dest)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[0] = (byte) ((0x0020 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0020) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenT_Connection_complete);
      return 0;
    }
    public int EIBOpenT_Connection (ushort dest)
    {
      if (EIBOpenT_Connection_async (dest) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenT_Group_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0022 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenT_Group_async (ushort dest, bool write_only,
				     AGARG_BOOLa (priority, ARG_NONE))
    {
      byte[]head = new byte[6];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((write_only) ? 0xff : 0);
      ibuf[5] = (byte) ((priority) & 0xff);
      ibuf[0] = (byte) ((0x0022 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0022) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenT_Group_complete);
      return 0;
    }
    public int EIBOpenT_Group (ushort dest, bool write_only,
			       AGARG_BOOLa (priority, ARG_NONE))
    {
      if (EIBOpenT_Group_async
	  (dest, write_only, ALARG_BOOLa (priority, ARG_NONE)) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenT_Individual_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0021 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenT_Individual_async (ushort dest, bool write_only)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      ibuf[4] = (byte) ((write_only) ? 0xff : 0);
      ibuf[0] = (byte) ((0x0021 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0021) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenT_Individual_complete);
      return 0;
    }
    public int EIBOpenT_Individual (ushort dest, bool write_only)
    {
      if (EIBOpenT_Individual_async (dest, write_only) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenT_TPDU_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0024 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenT_TPDU_async (ushort src)
    {
      byte[]head = new byte[5];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((src >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((src) & 0xff);
      ibuf[0] = (byte) ((0x0024 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0024) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenT_TPDU_complete);
      return 0;
    }
    public int EIBOpenT_TPDU (ushort src)
    {
      if (EIBOpenT_TPDU_async (src) == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenVBusmonitor_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0001)
	{
	  errno = EBUSY;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0012 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenVBusmonitor_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0012 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0012) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenVBusmonitor_complete);
      return 0;
    }
    public int EIBOpenVBusmonitor ()
    {
      if (EIBOpenVBusmonitor_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBOpenVBusmonitorText_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  == 0x0001)
	{
	  errno = EBUSY;
	  return -1;
	}
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0013 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBOpenVBusmonitorText_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0013 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0013) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBOpenVBusmonitorText_complete);
      return 0;
    }
    public int EIBOpenVBusmonitorText ()
    {
      if (EIBOpenVBusmonitorText_async () == -1)
	return -1;
      return EIBComplete ();
    }

    private int EIBReset_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0004 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      return 0;
    }
    public int EIBReset_async ()
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      ibuf[0] = (byte) ((0x0004 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0004) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIBReset_complete);
      return 0;
    }
    public int EIBReset ()
    {
      if (EIBReset_async () == -1)
	return -1;
      return EIBComplete ();
    }

    public int EIBSendAPDU (byte[]data)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      if (data.Length < 2)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + data.Length];
      sendlen = data.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < data.Length; i++)
	ibuf[i + head.Length] = data[i];
      ibuf[0] = (byte) ((0x0025 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0025) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      return sendlen;
    }

    public int EIBSendGroup (ushort dest, byte[]data)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      if (data.Length < 2)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + data.Length];
      sendlen = data.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < data.Length; i++)
	ibuf[i + head.Length] = data[i];
      ibuf[0] = (byte) ((0x0027 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0027) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      return sendlen;
    }

    public int EIBSendTPDU (ushort dest, byte[]data)
    {
      byte[]head = new byte[4];
      byte[]ibuf = head;
      ibuf[2] = (byte) ((dest >> 8) & 0xff);
      ibuf[2 + 1] = (byte) ((dest) & 0xff);
      if (data.Length < 2)
	throw new IndexOutOfRangeException ("data to short");
      ibuf = new byte[head.Length + data.Length];
      sendlen = data.Length;
      for (int i = 0; i < head.Length; i++)
	ibuf[i] = head[i];
      for (int i = 0; i < data.Length; i++)
	ibuf[i + head.Length] = data[i];
      ibuf[0] = (byte) ((0x0025 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0025) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      return sendlen;
    }

    private int EIB_State_Threads_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      if (((((((int) (data[0])) & 0xff) << 8) | ((((int) (data[1])) & 0xff))))
	  != 0x0101 || data.Length < 2)
	{
	  errno = ECONNRESET;
	  return -1;
	}
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_State_Threads_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[0] = (byte) ((0x0101 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0101) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_State_Threads_complete);
      return 0;
    }
    public int EIB_State_Threads (EIBBuffer buf)
    {
      if (EIB_State_Threads_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }
    private int EIB_State_Backends_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_State_Backends_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[0] = (byte) ((0x0102 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0102) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_State_Backends_complete);
      return 0;
    }
    public int EIB_State_Backends (EIBBuffer buf)
    {
      if (EIB_State_Backends_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }
    private int EIB_State_Servers_complete ()
    {
      complete = null;
      int i;
      if (_EIB_GetRequest () == -1)
	return -1;
      i = data.Length - 2;
      buf.data = new byte[i];
      for (int j = 0; j < i; j++)
	buf.data[j] = data[j + 2];
      return i;
    }
    public int EIB_State_Servers_async (EIBBuffer buf)
    {
      byte[]head = new byte[2];
      byte[]ibuf = head;
      this.buf = buf;
      ibuf[0] = (byte) ((0x0103 >> 8) & 0xff);
      ibuf[1] = (byte) ((0x0103) & 0xff);
      if (_EIB_SendRequest (ibuf) == -1)
	return -1;
      complete = new complete_t (EIB_State_Servers_complete);
      return 0;
    }
    public int EIB_State_Servers (EIBBuffer buf)
    {
      if (EIB_State_Servers_async (buf) == -1)
	return -1;
      return EIBComplete ();
    }
  }
}
