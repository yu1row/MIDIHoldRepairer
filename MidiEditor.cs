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
using MIDIHoldRepairer.Structures;
using System.IO;

namespace MIDIHoldRepairer
{
    internal class MidiEditor : IDisposable
    {
        private string _path = string.Empty;
        public MIDIData? MIDIData { get; private set; }
        public bool IsLoaded { get { return MIDIData?.IsLoaded ?? false; } }
        public bool IsEditable
        {
            get
            {
                return IsLoaded && ((MIDIData?.GetTimeMode()??-1) == 0);
            }
        }

        public int HoldIntervalThreshold
        { get
            {
                return (MIDIData?.GetTimeResolution() ?? 960) / 64;
            }
        }
        public string FilePath { get { return _path; } }
        public string FileName { get { return Path.GetFileName(FilePath); } }

        public string MIDIInformationsText
        {
            get
            {
                if (MIDIData != null)
                {
                    return $"Format = {MIDIData.Data.Format}, Time mode = {MIDIData.GetTimeMode()}, Time resolution = {MIDIData.GetTimeResolution()}, Track = {MIDIData.Data.NumTrack}";
                }
                else
                {
                    return "Failed to load.";
                }
            }
        }
        public bool Load(string path)
        {
            MIDIData?.UnLoad();
            MIDIData = new MIDIData(path);
            _path = path;
            return MIDIData.IsLoaded;
        }

        public bool Save()
        {
            return Save(this._path);
        }

        public bool Save(string path)
        {
            if (MIDIData != null)
            {
                var ret = MIDIDataLibWrapper.MIDIData_SaveAsSMF(MIDIData.Pointer, path);
                if (ret == 1)
                {
                    _path = path;
                    return true;
                }
            }
            return false;
        }
        public void UnLoad()
        {
            MIDIData?.UnLoad();
        }
        void IDisposable.Dispose()
        {
            UnLoad();
        }
        public static List<MIDIEvent> GetHoldEvents(MIDITrack? track)
        {
            var list = new List<MIDIEvent>();
            foreach(var e in track?.GetAllEvents() ?? Enumerable.Empty<MIDIEvent>())
            {
                if (!e.IsEvent(MIDIEvent.EventKinds.ControlChange)) { continue; }
                if (e.CData2 != 64) { continue; }
                list.Add(e);
            }
            return list;
        }

    }
}
