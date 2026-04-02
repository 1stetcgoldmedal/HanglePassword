using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation.Peers;
using System.Windows.Media;
using System.Globalization;

namespace HanglePassword
{
    public partial class SimpleHangulPassword : UserControl
    {
        private double _charWidth = 0;

        public SimpleHangulPassword()
        {
            InitializeComponent();

            DataObject.AddCopyingHandler(RealInputBox, CancelClipboardEvent);
            DataObject.AddPastingHandler(RealInputBox, CancelClipboardEvent);
            DataObject.AddSettingDataHandler(RealInputBox, CancelClipboardEvent);
        }

        private void RealInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MaskDisplay.Text = new string('＊', RealInputBox.Text.Length);
            UpdateVisuals(); // 글자가 바뀔 때마다 커서/셀렉션 위치 갱신
        }

        private void RealInputBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateVisuals(); // 마우스로 클릭하거나 드래그할 때 갱신
        }

        private void RealInputBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UpdateVisuals();
        }

        private void RealInputBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            FakeCaret.Visibility = Visibility.Hidden;
            FakeSelection.Visibility = Visibility.Collapsed;
        }

        // 💡 [핵심] 가짜 커서와 가짜 셀렉션의 위치를 동기화하는 통합 메서드
        private void UpdateVisuals()
        {
            if (_charWidth == 0) MeasureCharWidth();

            if (RealInputBox.SelectionLength > 0)
            {
                // 드래그(선택) 중일 때는 파란 블록을 보여주고 커서를 숨김
                FakeSelection.Visibility = Visibility.Visible;
                Canvas.SetLeft(FakeSelection, RealInputBox.SelectionStart * _charWidth);
                FakeSelection.Width = RealInputBox.SelectionLength * _charWidth;

                FakeCaret.Visibility = Visibility.Hidden;
            }
            else
            {
                // 선택 영역이 없을 때는 파란 블록을 숨기고, 커서를 현재 위치에 그림
                FakeSelection.Visibility = Visibility.Collapsed;

                if (RealInputBox.IsKeyboardFocusWithin)
                {
                    FakeCaret.Visibility = Visibility.Visible;
                    Canvas.SetLeft(FakeCaret, RealInputBox.CaretIndex * _charWidth);
                }
            }
        }

        private void MeasureCharWidth()
        {
            var typeface = new Typeface(MaskDisplay.FontFamily, MaskDisplay.FontStyle, MaskDisplay.FontWeight, MaskDisplay.FontStretch);
            var formattedText = new FormattedText(
                "＊",
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                typeface,
                MaskDisplay.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            _charWidth = formattedText.WidthIncludingTrailingWhitespace;
        }

        private void RealInputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.A) return;
                if (e.Key == Key.C || e.Key == Key.V || e.Key == Key.X) e.Handled = true;
            }
        }

        private void CancelClipboardEvent(object sender, DataObjectEventArgs e) => e.CancelCommand();

        public string Password => RealInputBox.Text;

        public string GetPasswordAndClear()
        {
            string pwd = RealInputBox.Text;
            RealInputBox.Clear();
            return pwd;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this) { };
        }
    }
}