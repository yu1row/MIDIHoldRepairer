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

namespace MIDIHoldRepairer
{
    internal static class MIDIDataLibWrapper
    {
        const string LIB_MIDIDATA_PATH = @"libs\MIDIData.dll";
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIData_LoadFromSMF(string path);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_SaveAsSMF(IntPtr pMIDIData, string path);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern void MIDIData_Delete(IntPtr pMIDIData);

        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_GetFormat(IntPtr pMIDIData);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_GetTimeMode(IntPtr pMIDIData);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_GetTimeResolution(IntPtr pMIDIData);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_CountTrack(IntPtr pMIDIData);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIData_GetTitle(IntPtr pMIDIData, StringBuilder buf, int len);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIData_GetTrack(IntPtr pMIDIData, int lTrackIndex);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIData_BreakTime(IntPtr pMIDIData, int time, out int measure, out int beat, out int tick);

        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetPrevTrack(IntPtr pMIDITrack);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetNextTrack(IntPtr pMIDITrack);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetName(IntPtr pMIDITrack, StringBuilder buf, int len);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetFirstEvent(IntPtr pMIDITrack);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetLastEvent(IntPtr pMIDITrack);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetFirstKindEvent(IntPtr pMIDITrack, int kind);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDITrack_GetLastKindEvent(IntPtr pMIDITrack, int kind);

        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIEvent_GetNextEvent(IntPtr pMIDIEvent);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIEvent_GetNextSameKindEvent(IntPtr pMIDIEvent);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern IntPtr MIDIEvent_ToString(IntPtr pMIDIEvent, StringBuilder buf, int len);
        [DllImport(LIB_MIDIDATA_PATH)]
        public static extern int MIDIEvent_SetTime(IntPtr pMIDIEvent, int time);
    }
}
