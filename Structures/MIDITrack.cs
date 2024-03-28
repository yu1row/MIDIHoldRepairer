/*
 * Copyright(C) 2024 yu1row
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System.Runtime.InteropServices;
using System.Text;

namespace MIDIHoldRepairer.Structures
{
    public class MIDITrack
    {
        public IntPtr RootPointer { get; set; }
        public IntPtr Pointer { get; private set; }
        public StructMIDITrack Data { get; private set; }
        public MIDITrack(IntPtr pointer)
        {
            if (pointer != IntPtr.Zero)
            {
                Data = (StructMIDITrack)(Marshal.PtrToStructure(pointer, typeof(StructMIDITrack)) ?? new StructMIDITrack());
                RootPointer = Data.Parent;
            }
            Pointer = pointer;
        }
        public string GetTrackName()
        {
            if (Pointer != IntPtr.Zero)
            {
                var buf = new StringBuilder(2048);
                MIDIDataLibWrapper.MIDITrack_GetName(Pointer, buf, buf.Capacity);
                return buf.ToString();
            }
            return string.Empty;
        }
        public MIDITrack? GetPrevTrack()
        {
            if (Data.PrevTrack != IntPtr.Zero)
            {
                return new MIDITrack(Data.PrevTrack);
            }
            return null;
        }

        public MIDITrack? GetNextTrack()
        {
            if (Data.NextTrack != IntPtr.Zero)
            {
                return new MIDITrack(Data.NextTrack);
            }
            return null;
        }

        public MIDIEvent? GetFirstEvent()
        {
            var p = MIDIDataLibWrapper.MIDITrack_GetFirstEvent(Pointer);
            if (p != IntPtr.Zero)
            {
                return new MIDIEvent(p, Pointer, RootPointer);
            }
            return null;
        }
        public MIDIEvent? GetFirstKindEvent(int kind)
        {
            IntPtr p = MIDIDataLibWrapper.MIDITrack_GetLastKindEvent(Pointer, kind);
            if (p != IntPtr.Zero)
            {
                return new MIDIEvent(p, Pointer, RootPointer);
            }
            return null;
        }

        public MIDIEvent? GetLastEvent()
        {
            var p = MIDIDataLibWrapper.MIDITrack_GetLastEvent(Pointer);
            if (p != IntPtr.Zero)
            {
                return new MIDIEvent(p, Pointer, RootPointer);
            }
            return null;
        }
        public List<MIDIEvent> GetAllEvents()
        {
            var list = new List<MIDIEvent>();
            var ev = GetFirstEvent();
            if (ev != null)
            {
                do {
                    list.Add(ev);
                    ev = ev.GetNextEvent();
                } while (ev != null);
            }
            return list;
        }
        public List<MIDIEvent> GetEvents(MIDIEvent.EventKinds kind)
        {
            var list = new List<MIDIEvent>();
            var ev = GetFirstKindEvent((int)kind);
            if (ev != null)
            {
                do {
                    list.Add(ev);
                    ev = ev.GetNextSameKindEvent();
                } while (ev != null);
            }
            return list;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StructMIDITrack
        {
            /// <summary>
            /// 一時インデックス
            /// </summary>
            public int TempIndex { get; set; }
            /// <summary>
            /// トラック内のイベント数
            /// </summary>
            public int NumEvent { get; set; }
            /// <summary>
            /// 最初のイベントへのポインタ
            /// </summary>
            public IntPtr FirstEvent { get; set; }
            /// <summary>
            /// 最後のイベントへのポインタ
            /// </summary>
            public IntPtr LastEvent { get; set; }
            /// <summary>
            /// 前のトラック
            /// </summary>
            public IntPtr PrevTrack { get; set; }
            /// <summary>
            /// 次のトラック
            /// </summary>
            public IntPtr NextTrack { get; set; }
            /// <summary>
            /// 親(MIDIData)
            /// </summary>
            public IntPtr Parent { get; set; }
            /// <summary>
            /// (0=OFF,1=ON)
            /// </summary>
            int InputOn { get; set; }
            /// <summary>
            /// 入力ポート(0～255)
            /// </summary>
            public int InputPort { get; set; }
            /// <summary>
            /// 入力チャンネル(-1=n/a, 0～15)
            /// </summary>
            public int InputChannel { get; set; }
            /// <summary>
            /// 出力(0=OFF, 1=ON)
            /// </summary>
            public int OutputOn { get; set; }
            /// <summary>
            /// 出力ポート(0～255)
            /// </summary>
            public int OutputPort { get; set; }
            /// <summary>
            /// 出力チャンネル(-1=n/a, 0～15)
            /// </summary>
            public int OutputChannel { get; set; }
            /// <summary>
            /// タイム+
            /// </summary>
            public int TimePlus { get; set; }
            /// <summary>
            /// キー+
            /// </summary>
            public int KeyPlus { get; set; }
            /// <summary>
            /// ベロシティ+
            /// </summary>
            public int VelPlus { get; set; }
            /// <summary>
            /// 表示モード(0=通常,1=ドラム)
            /// </summary>
            public int ViewMode { get; set; }
            /// <summary>
            /// 前景色
            /// </summary>
            public int ForeColor { get; set; }
            /// <summary>
            /// 背景色
            /// </summary>
            public int BackColor { get; set; }
            /// <summary>
            /// ユーザー用自由領域1
            /// </summary>
            public int User1 { get; set; }
            /// <summary>
            /// ユーザー用自由領域2
            /// </summary>
            public int User2 { get; set; }
            /// <summary>
            /// ユーザー用自由領域3\
            /// </summary>
            public int User3 { get; set; }
            /// <summary>
            /// ユーザー用フラグ
            /// </summary>
            public int UserFlag { get; set; }
        }
    }
}
