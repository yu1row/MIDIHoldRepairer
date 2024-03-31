[English](README.md) | 日本語

# MIDI Hold Repairer

このアプリケーションは、MIDIファイル内のペダルイベント(コントロールチェンジ64番)のうち、狭すぎるOn/Offイベントのタイミングを修復します。

## 外観

### メインウインドウ
![](docs/ScreenShot.png)
* 上部枠
以下のMIDIファイル情報を表示します。
  * Format: SMF(Standard MIDI File)フォーマット(=0,1,2)
  * Time mode: タイムベース(=0:TPQN, 24:SMPTE24, 25:SMPTE25, 29:SMPTE29, 30:SMPTE30)
    > **_注:_**
    > このアプリケーションはTPQNモードのみをサポートします。
  * Time resolution: ４分音符あたりのティック数
  * Track: ファイル内のトラック数
* 左枠
ファイル内のトラックリスト
* 右枠
選択されたトラック内のペダルイベント(Hold1 = コントロールチェンジ64番)のリスト
  * Time: `小節数:拍数:ティック数`
    > **_タイミングが短すぎる場合、赤字になります。_**
  * On/Off: Indicates pedal on/off
  * Diff: 次のペダルオフイベントまでのティック数
    > **_ティック数が少なすぎる場合、赤字になります。_**
  * Repair selected ボタン
    選択されたイベントのタイミングを修復します。
    > **_修復可能なイベントが選択されていない場合、このボタンは押せません。_**
  * Repair all ボタン
    リスト内のすべてのイベントのタイミングを修復します。
    > **_リスト内に修復可能なイベントがない場合、このボタンは押せません。_**

## サードパーティーライブラリについて
このアプリケーションでは、「[MIDIDataライブラリ8.0](https://openmidiproject.opal.ne.jp/MIDIDataLibrary.html)」を使用します。