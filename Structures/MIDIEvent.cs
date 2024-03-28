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
    public class MIDIEvent
    {
        public IntPtr RootPointer { get; set; }
        public IntPtr TrackPointer { get; set; }
        public IntPtr Pointer { get; private set; }
        public StructMIDIEvent Data { get; private set; }
        public int CData1 { get { return BitConverter.GetBytes(Data.Data)[0]; } }
        public int CData2 { get { return BitConverter.GetBytes(Data.Data)[1]; } }
        public int CData3 { get { return BitConverter.GetBytes(Data.Data)[2]; } }
        public int CData4 { get { return BitConverter.GetBytes(Data.Data)[3]; } }
        public MIDIEvent(IntPtr pointer, IntPtr trackPointer, IntPtr rootPointer)
        {
            if (pointer != IntPtr.Zero)
            {
                Data = (StructMIDIEvent)(Marshal.PtrToStructure(pointer, typeof(StructMIDIEvent)) ?? new StructMIDIEvent());
                RootPointer = rootPointer;
                TrackPointer = trackPointer;
            }
            Pointer = pointer;
        }
        public string GetTimeString()
        {
            int measure, beat, tick;
            MIDIDataLibWrapper.MIDIData_BreakTime(RootPointer, Data.Time, out measure, out beat, out tick);
            return $"{measure+1:00000}:{beat+1:00}:{tick:00}";
        }
        public string GetString()
        {
            var buf = new StringBuilder(2048);
            MIDIDataLibWrapper.MIDIEvent_ToString(Pointer, buf, buf.Capacity);
            return buf.ToString();
        }
        public MIDIEvent? GetNextEvent()
        {
            var p = MIDIDataLibWrapper.MIDIEvent_GetNextEvent(Pointer);
            if (p != IntPtr.Zero)
            {
                return new MIDIEvent(p, TrackPointer, RootPointer);
            }
            return null;
        }
        public MIDIEvent? GetNextSameKindEvent()
        {
            var p = MIDIDataLibWrapper.MIDIEvent_GetNextSameKindEvent(Pointer);
            if (p != IntPtr.Zero)
            {
                return new MIDIEvent(p, TrackPointer, RootPointer);
            }
            return null;
        }
        public bool IsEvent(EventKinds kind)
        {
            return ( (int)kind <= Data.Kind && Data.Kind <= ((int)kind+15) );
        }
        public enum EventKinds: int
        {
            /// <summary>
            /// コントロールチェンジ
            /// </summary>
            ControlChange = 0xB0,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct StructMIDIEvent
        {
            /// <summary>
            /// 一時インデックス
            /// </summary>
            public int TempIndex { get; set; }
            /// <summary>
            /// このMIDIイベントの置かれている時刻
            /// </summary>
            public int Time { get; set; }
            /// <summary>
            /// MIDIイベントの種類を表す識別番号(MIDIEVENT_SEQUENCENUMBER(0x00)～MIDIEVENT_SYSEXCONTINUE(0xF7)の値)
            /// </summary>
            public int Kind { get; set; }
            /// <summary>
            /// データ部の長さ[バイト]
            /// </summary>
            public int Len { get; set; }
            /// <summary>
            /// データ部へのポインタ
            /// </summary>
            IntPtr PData { get; set; }
            /// <summary>
            /// データ部(4バイト:MIDIチャンネルイベントの場合)
            /// </summary>
            public uint Data { get; set; }
            /// <summary>
            /// 次のMIDIイベント
            /// </summary>
            public IntPtr NextEvent { get; set; }
            /// <summary>
            /// 前のMIDIイベント
            /// </summary>
            public IntPtr PrevEvent { get; set; }
            /// <summary>
            /// 次の同じ種類のMIDIイベントへのポインタ
            /// </summary>
            public IntPtr NextSameKindEvent { get; set; }
            /// <summary>
            /// 前の同じ種類のMIDIイベントへのポインタ
            /// </summary>
            public IntPtr PrevSameKindEvent { get; set; }
            /// <summary>
            /// 次の結合イベント保持用ポインタ
            /// </summary>
            public IntPtr NextCombinedEvent { get; set; }
            /// <summary>
            /// 前の結合イベント保持用ポインタ
            /// </summary>
            public IntPtr PrevCombinedEvent { get; set; }
            /// <summary>
            /// 親(MIDITrack)へのポインタ
            /// </summary>
            public IntPtr Parent { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできる拡張領域その1(MIDIファイルへの保存なし)
            /// </summary>
            public int User1 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできる拡張領域その2(MIDIファイルへの保存なし)
            /// </summary>
            public int User2 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできる拡張領域その3(MIDIファイルへの保存なし)
            /// </summary>
            public int User3 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできるフラグのための拡張領域(MIDIファイルへの保存なし)
            /// </summary>
            public int UserFlag { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできるポインタ格納領域その1(MIDIファイルへの保存なし)
            /// </summary>
            public IntPtr PUser1 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできるポインタ格納領域その2(MIDIファイルへの保存なし)
            /// </summary>
            public IntPtr PUser2 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできるポインタ格納領域その3(MIDIファイルへの保存なし)
            /// </summary>
            public IntPtr PUser3 { get; set; }
            /// <summary>
            /// ユーザーが自由に使うことのできるポインタ格納領域その4(MIDIファイルへの保存なし)
            /// </summary>
            public IntPtr PUser4 { get; set; }
        }
    }
}
