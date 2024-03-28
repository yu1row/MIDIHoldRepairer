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
using Microsoft.Win32;
using MIDIHoldRepairer.Structures;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MIDIHoldRepairer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly static RoutedCommand RepairAllCommand = new RoutedCommand("RepairAllCommand", typeof(MainWindow));
        public readonly static RoutedCommand RepairSelectedCommand = new RoutedCommand("RepairSelectedCommand", typeof(MainWindow));

        private readonly MidiEditor _editor;
        public List<HoldEvent> _hold_events;

        public MainWindow()
        {
            InitializeComponent();
            _editor = new MidiEditor();
            _hold_events = new List<HoldEvent>();
            DataGridHoldEvents.ItemsSource = _hold_events;
        }

        private IntPtr? GetCurrentEventPointer()
        {
            IntPtr? ret = null;
            var index = DataGridHoldEvents.SelectedIndex;
            if (0 < index)
            {
                ret = _hold_events[index].Event.Pointer; ;
            }
            return ret;
        }
        private void SelectEventByPointer(IntPtr? pointer)
        {
            if (pointer != null)
            {
                foreach (var ev in _hold_events)
                {
                    if (ev.Event.Pointer == pointer)
                    {
                        DataGridHoldEvents.SelectedIndex = DataGridHoldEvents.Items.IndexOf(ev);
                        break;
                    }
                }
            }
        }
        private void UpdateEventTable(MIDITrack? track = null)
        {
            track ??= _editor?.MIDIData?.GetTrack(ListTracks.SelectedIndex);
            _hold_events.Clear();
            if (_editor?.IsEditable ?? false)
            {
                var events = MidiEditor.GetHoldEvents(track);
                foreach (var ev in events)
                {
                    _hold_events.Add(new HoldEvent(ev));
                }
                var lastHoldOnTime = -1;
                var t = _editor.HoldIntervalThreshold;
                foreach (HoldEvent ev in Enumerable.Reverse(_hold_events))
                {
                    ev.IsShortDiff = false;
                    if (ev.IsOn)
                    {
                        lastHoldOnTime = ev.Event.Data.Time;
                    }
                    else if (t <= lastHoldOnTime)
                    {
                        ev.TimeDiff = lastHoldOnTime - ev.Event.Data.Time;
                        ev.IsShortDiff = (0 <= ev.TimeDiff && ev.TimeDiff < t);
                    }
                }
            }
            DataGridHoldEvents.Items.Refresh();
        }
        private void UpdateWindow()
        {
            var fileName = _editor.IsLoaded ? $" - {_editor.FileName}" : string.Empty;
            this.Title = $"{App.Current.Resources["AppName"]}{fileName}";
            label_filepath.Content = _editor.IsLoaded ? _editor.FilePath : string.Empty;
            label_fileinfo.Content = _editor.IsLoaded ? $" - {_editor.MIDIInformationsText}" : string.Empty;        }

        private void UpdateTrackList()
        {
            ListTracks.Items.Clear();
            var num = 0;
            foreach(var track in _editor.MIDIData?.GetAllTracks()??Enumerable.Empty<MIDITrack>())
            {
                ListTracks.Items.Add($"[{num++}] - {track.GetTrackName()}");
            }
        }
        private void SelectFirstTrack()
        {
            if (ListTracks.Items.Count > 0)
            {
                ListTracks.SelectedIndex = 0;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _editor.UnLoad();
        }

        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "MIDI File (*.mid)|*.mid|All file (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                if (!_editor.Load(dialog.FileName))
                {
                    MessageBox.Show(this, "Failed to load.", "Error");
                }
                UpdateWindow();
                UpdateTrackList();
                SelectFirstTrack();
            }
        }
        private void CommandBinding_RepairAll(object sender, ExecutedRoutedEventArgs e)
        {
            var p = GetCurrentEventPointer();
            var t = _editor.HoldIntervalThreshold;
            foreach (var ev in _hold_events)
            {
                if (ev.IsShortDiff)
                {
                    var newTime = ev.Event.Data.Time - (t - ev.TimeDiff);
                    MIDIDataLibWrapper.MIDIEvent_SetTime(ev.Event.Pointer, newTime);
                }
            }
            UpdateEventTable();
            SelectEventByPointer(p);
        }
        private void CommandBinding_RepairSelected(object sender, ExecutedRoutedEventArgs e)
        {
            var index = DataGridHoldEvents.SelectedIndex;
            if (0 < index)
            {
                var p = GetCurrentEventPointer();
                var ev = _hold_events[index];
                var newTime = ev.Event.Data.Time - (_editor.HoldIntervalThreshold - ev.TimeDiff);
                MIDIDataLibWrapper.MIDIEvent_SetTime(ev.Event.Pointer, newTime);
                UpdateEventTable();
                SelectEventByPointer(p);
            }
        }
        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (_editor.IsLoaded)
            {
                if (!_editor.Save())
                {
                    MessageBox.Show(this, "Failed to save.", "Error");
                }
            }
        }
        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _editor.IsLoaded;
        }
        private void CanRepairAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (_hold_events != null)
            {
                foreach (var ev in _hold_events)
                {
                    if (ev.IsShortDiff)
                    {
                        e.CanExecute = true;
                        break;
                    }
                }
            }
        }
        private void CanRepairSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (_hold_events != null && 0 < _hold_events.Count)
            {
                var item = DataGridHoldEvents.SelectedItem;
                if (item != null && item is HoldEvent && ((HoldEvent)item).IsShortDiff)
                {
                    e.CanExecute = true;
                }
            }
        }

        private void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            if (_editor.IsLoaded)
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "MIDI File (*.mid)|*.mid|All file (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    if (!_editor.Save(dialog.FileName))
                    {
                        MessageBox.Show(this, "Failed to save.", "Error");
                    }
                }
            }
            UpdateWindow();
        }
        private void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void ListTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEventTable();
        }
    }
}