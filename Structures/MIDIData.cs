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

namespace MIDIHoldRepairer.Structures
{
    public class MIDIData
    {
        public IntPtr Pointer { get; private set; }
        public StructMIDIData Data { get; private set; }
        public bool IsLoaded { get { return Pointer != IntPtr.Zero; } }
        public MIDIData()
        {
            Pointer = IntPtr.Zero;
        }

        public MIDIData(IntPtr pointer)
        {
            if (pointer != IntPtr.Zero)
            {
                Data = (StructMIDIData)(Marshal.PtrToStructure(pointer, typeof(StructMIDIData)) ?? new StructMIDIData());
            }
            Pointer = pointer;
        }

        public MIDIData(string path): this(MIDIDataLibWrapper.MIDIData_LoadFromSMF(path)) { }

        public bool Save(string path)
        {
            int ret = 0;
            if (IsLoaded)
            {
                ret = MIDIDataLibWrapper.MIDIData_SaveAsSMF(Pointer, path);
            }
            return ret == 1;
        }
        public int GetTimeMode()
        {
            int ret = -1;
            if (IsLoaded)
            {
                ret = MIDIDataLibWrapper.MIDIData_GetTimeMode(Pointer);
            }
            return ret;
        }
        public int GetTimeResolution()
        {
            int ret = -1;
            if (IsLoaded)
            {
                ret = MIDIDataLibWrapper.MIDIData_GetTimeResolution(Pointer);
            }
            return ret;
        }

        public MIDITrack? GetFirstTrack()
        {
            if (Data.FirstTrack != IntPtr.Zero)
            {
                return new MIDITrack(Data.FirstTrack);
            }
            return null;
        }
        public MIDITrack? GetTrack(int index)
        {
            if (0 <= index)
            {
                IntPtr p = MIDIDataLibWrapper.MIDIData_GetTrack(Pointer, index);
                if (p != IntPtr.Zero)
                {
                    return new MIDITrack(p);
                }
            }
            return null;
        }
        public void UnLoad()
        {
            if (IsLoaded)
            {
                MIDIDataLibWrapper.MIDIData_Delete(Pointer);
                Pointer = IntPtr.Zero;
            }
        }
        public List<MIDITrack> GetAllTracks()
        {
            var list = new List<MIDITrack>();
            if (IsLoaded)
            {
                var track = GetFirstTrack();
                if (track != null)
                {
                    do {
                        list.Add(track);
                        track = track.GetNextTrack();
                    } while (track != null);
                }

            }
            return list;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct StructMIDIData
        {
            /// <summary>
            /// MIDIデータのフォーマット(0～2)
            /// </summary>
            public uint Format { get; set; }
            /// <summary>
            /// MIDIデータ内のトラック数
            /// </summary>
            public uint NumTrack { get; set; }
            /// <summary>
            /// MIDIデータのタイムベース
            /// ((256 - タイムモード) << 8) + タイムレゾリューション
            /// </summary>
            public uint TimeBase { get; set; }
            /// <summary>
            /// 最初のMIDIトラック
            /// </summary>
            public IntPtr FirstTrack { get; set; }
            /// <summary>
            /// 最後のMIDIトラック
            /// </summary>
            public IntPtr LastTrack { get; set; }
            /// <summary>
            /// 次のシーケンス
            /// </summary>
            public IntPtr NextSeq { get; set; }
            /// <summary>
            /// 前のシーケンス
            /// </summary>
            public IntPtr PrevSeq { get; set; }
            /// <summary>
            /// 親(常にNULL)
            /// </summary>
            public IntPtr Parent { get; set; }
            /// <summary>
            /// 予約領域1(使用禁止)
            /// </summary>
            public int Reserved1 { get; set; }
            /// <summary>
            /// 予約領域2(使用禁止)
            /// </summary>
            public int Reserved2 { get; set; }
            /// <summary>
            /// 予約領域3(使用禁止)
            /// </summary>
            public int Reserved3 { get; set; }
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
