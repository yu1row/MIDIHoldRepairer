[English](README.md) | ���{��

# MIDI Hold Repairer

���̃A�v���P�[�V�����́AMIDI�t�@�C�����̃y�_���C�x���g(�R���g���[���`�F���W64��)�̂����A��������On/Off�C�x���g�̃^�C�~���O���C�����܂��B

## �O��

### ���C���E�C���h�E
![](docs/ScreenShot.png)
* �㕔�g
�ȉ���MIDI�t�@�C������\�����܂��B
  * Format: SMF(Standard MIDI File)�t�H�[�}�b�g(=0,1,2)
  * Time mode: �^�C���x�[�X(=0:TPQN, 24:SMPTE24, 25:SMPTE25, 29:SMPTE29, 30:SMPTE30)
    > **_��:_**
    > ���̃A�v���P�[�V������TPQN���[�h�݂̂��T�|�[�g���܂��B
  * Time resolution: �S������������̃e�B�b�N��
  * Track: �t�@�C�����̃g���b�N��
* ���g
�t�@�C�����̃g���b�N���X�g
* �E�g
�I�����ꂽ�g���b�N���̃y�_���C�x���g(Hold1 = �R���g���[���`�F���W64��)�̃��X�g
  * Time: `���ߐ�:����:�e�B�b�N��`
    > **_�^�C�~���O���Z������ꍇ�A�Ԏ��ɂȂ�܂��B_**
  * On/Off: Indicates pedal on/off
  * Diff: ���̃y�_���I�t�C�x���g�܂ł̃e�B�b�N��
    > **_�e�B�b�N�������Ȃ�����ꍇ�A�Ԏ��ɂȂ�܂��B_**
  * Repair selected �{�^��
    �I�����ꂽ�C�x���g�̃^�C�~���O���C�����܂��B
    > **_�C���\�ȃC�x���g���I������Ă��Ȃ��ꍇ�A���̃{�^���͉����܂���B_**
  * Repair all �{�^��
    ���X�g���̂��ׂẴC�x���g�̃^�C�~���O���C�����܂��B
    > **_���X�g���ɏC���\�ȃC�x���g���Ȃ��ꍇ�A���̃{�^���͉����܂���B_**

## �T�[�h�p�[�e�B�[���C�u�����ɂ���
���̃A�v���P�[�V�����ł́A�u[MIDIData���C�u����8.0](https://openmidiproject.opal.ne.jp/MIDIDataLibrary.html)�v���g�p���܂��B