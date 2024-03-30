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

namespace MIDIHoldRepairer
{
    public class HoldEvent
    {
        public HoldEvent(MIDIEvent holdEvent) {
            Time = holdEvent.GetTimeString();
            OnOff = holdEvent.CData3 < 64 ? "Off" : "On";
            Text = holdEvent.GetString();
            TimeDiff = -1;
            Event = holdEvent;
        }
        public string Time { get; set; }
        public string OnOff { get; set; }
        public string Text { get; set; }
        public int TimeDiff { get; set; }
        public string TimeDiffDisp { get { return 0 <= TimeDiff ? TimeDiff.ToString() : ""; } }
        public MIDIEvent Event { get; set; }
        public bool IsOn { get { return (OnOff == "On"); } }
        public bool IsShortDiff { get; set; }
    }
}
