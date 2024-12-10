using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NAudio.Wave
{
    class WaveWindowNative : /*System.Windows.Forms.*/NativeWindow
    {
        private WaveInterop.WaveCallback waveCallback;

        public WaveWindowNative(WaveInterop.WaveCallback waveCallback)
        {
            this.waveCallback = waveCallback;
        }

        protected /*override*/ void WndProc(ref WaveInterop.WaveMessage m)
        {
            WaveInterop.WaveMessage message = default;//(WaveInterop.WaveMessage)m.Msg;
            
            switch(message)
            {
                case WaveInterop.WaveMessage.WaveOutDone:
                case WaveInterop.WaveMessage.WaveInData:
                    IntPtr hOutputDevice = default;//m.WParam;
                    WaveHeader waveHeader = new WaveHeader();
                    Marshal.PtrToStructure(/*m.LParam*/default, waveHeader);
                    waveCallback(hOutputDevice, message, IntPtr.Zero, waveHeader, IntPtr.Zero);
                    break;
                case WaveInterop.WaveMessage.WaveOutOpen:
                case WaveInterop.WaveMessage.WaveOutClose:
                case WaveInterop.WaveMessage.WaveInClose:
                case WaveInterop.WaveMessage.WaveInOpen:
                    waveCallback(/*m.WParam*/default, message, IntPtr.Zero, null, IntPtr.Zero);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        internal void AssignHandle(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal void ReleaseHandle()
        {
            throw new NotImplementedException();
        }
    }

    class WaveWindow : Form
    {
        internal IntPtr Handle;
        private WaveInterop.WaveCallback waveCallback;

        public WaveWindow(WaveInterop.WaveCallback waveCallback)
        {
            this.waveCallback = waveCallback;
        }

        protected /*override*/ void WndProc(ref WaveInterop.WaveMessage m)
        {
            WaveInterop.WaveMessage message = default;//(WaveInterop.WaveMessage)m.Msg;
            
            switch(message)
            {
                case WaveInterop.WaveMessage.WaveOutDone:
                case WaveInterop.WaveMessage.WaveInData:
                    IntPtr hOutputDevice = default;//m.WParam;
                    WaveHeader waveHeader = new WaveHeader();
                    Marshal.PtrToStructure(/*m.LParam*/default, waveHeader);
                    waveCallback(hOutputDevice, message, IntPtr.Zero, waveHeader, IntPtr.Zero);
                    break;
                case WaveInterop.WaveMessage.WaveOutOpen:
                case WaveInterop.WaveMessage.WaveOutClose:
                case WaveInterop.WaveMessage.WaveInClose:
                case WaveInterop.WaveMessage.WaveInOpen:
                    waveCallback(/*m.WParam*/default, message, IntPtr.Zero, null, IntPtr.Zero);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        internal void CreateControl()
        {
            throw new NotImplementedException();
        }
    }
}
