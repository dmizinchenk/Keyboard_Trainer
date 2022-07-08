using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Test
{
    //класс для связывания с XAML (для вывода скорости и количества ошибок)
    class Source : INotifyPropertyChanged 
    {
        int speed, fails;

        public int Fails
        {
            get => fails;
            set
            {
                if (!fails.Equals(value))
                {
                    fails = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Fails)));
                }
            }
        }
        public int Speed
        {
            get => speed;
            set
            {
                if (!speed.Equals(value))
                {
                    speed = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Speed)));
                }
            }
        }
        void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int seconds;
        bool mistake;
        bool isUpper;
        bool IsUpper 
        { 
            get => isUpper;
            set
            {
                if (isUpper != value)
                {
                    isUpper = value;
                    InitializeSymbolsButton(); //метод для отображения буквенных клавиш (большими/маленькими)
                }
            }
        }
        //метод для отображения буквенных клавиш (большими/маленькими)
        void InitializeSymbolsButton()
        {
            if (!isUpper)
            {
                btnA.Content = MyResource.a;
                btnB.Content = MyResource.b;
                btnC.Content = MyResource.c;
                btnD.Content = MyResource.d;
                btnE.Content = MyResource.e;
                btnF.Content = MyResource.f;
                btnG.Content = MyResource.g;
                btnH.Content = MyResource.h;
                btnI.Content = MyResource.i;
                btnJ.Content = MyResource.j;
                btnK.Content = MyResource.k;
                btnL.Content = MyResource.l;
                btnM.Content = MyResource.m;
                btnN.Content = MyResource.n;
                btnO.Content = MyResource.o;
                btnP.Content = MyResource.p;
                btnQ.Content = MyResource.q;
                btnR.Content = MyResource.r;
                btnS.Content = MyResource.s;
                btnT.Content = MyResource.t;
                btnU.Content = MyResource.u;
                btnV.Content = MyResource.v;
                btnW.Content = MyResource.w;
                btnX.Content = MyResource.x;
                btnY.Content = MyResource.y;
                btnZ.Content = MyResource.z;
            }
            else
            {
                btnA.Content = MyResource_upper.a;
                btnB.Content = MyResource_upper.b;
                btnC.Content = MyResource_upper.c;
                btnD.Content = MyResource_upper.d;
                btnE.Content = MyResource_upper.e;
                btnF.Content = MyResource_upper.f;
                btnG.Content = MyResource_upper.g;
                btnH.Content = MyResource_upper.h;
                btnI.Content = MyResource_upper.i;
                btnJ.Content = MyResource_upper.j;
                btnK.Content = MyResource_upper.k;
                btnL.Content = MyResource_upper.l;
                btnM.Content = MyResource_upper.m;
                btnN.Content = MyResource_upper.n;
                btnO.Content = MyResource_upper.o;
                btnP.Content = MyResource_upper.p;
                btnQ.Content = MyResource_upper.q;
                btnR.Content = MyResource_upper.r;
                btnS.Content = MyResource_upper.s;
                btnT.Content = MyResource_upper.t;
                btnU.Content = MyResource_upper.u;
                btnV.Content = MyResource_upper.v;
                btnW.Content = MyResource_upper.w;
                btnX.Content = MyResource_upper.x;
                btnY.Content = MyResource_upper.y;
                btnZ.Content = MyResource_upper.z;
            }
        }

        // список симовлов для каждого уровня сложности
        List<string> comboDifficult = new List<string>
        {
            {"alskdjfhg"}, {"zmxncbv"}, { "qpwoeiruty" }, { "1029384756" }, { @"`[];',./\" }
        };
        public MainWindow()
        {
            InitializeComponent();
            InitializeSymbolsButton();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            seconds = 0;
            if(Console.CapsLock == true)
                IsUpper = true;
            else
                IsUpper = false;
            DataContext = new Source();
        }
        
        //закрываем доступ к настройкам, обнуляем прошлую успеваемость
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            sliderDifficult.IsEnabled = false;
            registre.IsEnabled = false;
            (DataContext as Source).Speed = 0;
            (DataContext as Source).Fails = 0;
            genString.Text = userInput.Text = "";

            string collectionSymbols = "";
            Random rand = new Random();

            for (int i = 0; i < sliderDifficult.Value; i++)
            {
                collectionSymbols += comboDifficult[i];
            }

            string generateForOutput = "";
            for (int i = 0; i < 100; i++)
            {
                if ((i + 1) % 6 == 0)
                {
                    generateForOutput += " ";
                    continue;
                }
                if (registre.IsChecked == true && rand.Next(0, 2) > 0)
                    generateForOutput += collectionSymbols[rand.Next(collectionSymbols.Length)].ToString().ToUpper();
                else
                    generateForOutput += collectionSymbols[rand.Next(collectionSymbols.Length)];
            }
            genString.Text = generateForOutput;
        }

        //сбрасываем все результаты и открываем настройки
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            sliderDifficult.IsEnabled = true;
            registre.IsEnabled = true;
            genString.Text = userInput.Text = "";
            (DataContext as Source).Speed = 0;
            (DataContext as Source).Fails = 0;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            if (!btnStart.IsEnabled)
            {
                //начинаем отсчёт времени с нажатия любой кнопки
                if (!timer.IsEnabled)
                {
                    timer.Start();
                    seconds = 0;
                }
                //определяем нажатую клавишу, обеспечиваем ее функционал, и задаем стиль для неё
                switch (e.Key)
                {
                    case Key.Back:
                        btnBack.Style = this.Resources["btnOtherPress"] as Style;
                        if (userInput.Text.Length > 0)
                            userInput.Text = userInput.Text.Remove(userInput.Text.Length - 1);
                        break;
                    case Key.Enter:
                        btnEnter.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.Space:
                        btnSpace.Style = this.Resources["btnSpacePress"] as Style;
                        if (mistake == false)
                            userInput.Text += " ";
                        break;
                    case Key.D0:
                        btn0.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "0";
                        break;
                    case Key.D1:
                        btn1.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "1";
                        break;
                    case Key.D2:
                        btn2.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "2";
                        break;
                    case Key.D3:
                        btn3.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "3";
                        break;
                    case Key.D4:
                        btn4.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "4";
                        break;
                    case Key.D5:
                        btn5.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            userInput.Text += "5";
                        break;
                    case Key.D6:
                        btn6.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            userInput.Text += "6";
                        break;
                    case Key.D7:
                        btn7.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "7";
                        break;
                    case Key.D8:
                        btn8.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "8";
                        break;
                    case Key.D9:
                        btn9.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "9";
                        break;
                    case Key.NumPad0:
                        btn0.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "0";
                        break;
                    case Key.NumPad1:
                        btn1.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "1";
                        break;
                    case Key.NumPad2:
                        btn2.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "2";
                        break;
                    case Key.NumPad3:
                        btn3.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "3";
                        break;
                    case Key.NumPad4:
                        btn4.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "4";
                        break;
                    case Key.NumPad5:
                        btn5.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            userInput.Text += "5";
                        break;
                    case Key.NumPad6:
                        btn6.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            userInput.Text += "6";
                        break;
                    case Key.NumPad7:
                        btn7.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "7";
                        break;
                    case Key.NumPad8:
                        btn8.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "8";
                        break;
                    case Key.NumPad9:
                        btn9.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "9";
                        break;
                    case Key.LeftShift:
                        btnLeftShift.Style = this.Resources["btnOtherPress"] as Style;
                        IsUpper = true;
                        if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock) == true)
                            IsUpper = !IsUpper;
                        break;
                    case Key.RightShift:
                        btnRightShift.Style = this.Resources["btnOtherPress"] as Style;
                        IsUpper = true;
                        if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock) == true)
                            IsUpper = !IsUpper;
                        break;
                    case Key.CapsLock:
                        IsUpper = !IsUpper;
                        if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock))
                        {
                            btnCaps.Style = this.Resources["btnOtherPress"] as Style;
                        }
                        else
                        {
                            btnCaps.Style = this.Resources["btnOther"] as Style;
                        }
                        break;
                    case Key.LeftCtrl:
                        btnLeftCtrl.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.RightCtrl:
                        btnRightCtrl.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.System:
                        btnRightAlt.Style = btnLeftAlt.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.Oem1: //точка с запятой
                        btnSemicolon.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += ";";
                        break;
                    case Key.OemPlus: //Равно
                        btnPlus.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "=";
                        break;
                    case Key.OemComma:
                        btnComma.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += ",";
                        break;
                    case Key.OemMinus:
                        btnMinus.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "-";
                        break;
                    case Key.OemPeriod: //Точка
                        btnPoint.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            userInput.Text += ".";
                        break;
                    case Key.OemQuestion: //слеш
                        btnSlash.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "/";
                        break;
                    case Key.Oem3: //Тильда
                        btnTilda.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "`";
                        break;
                    case Key.Oem5: //Бекслеш
                        btnBackSlash.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += @"\";
                        break;
                    case Key.Oem6: //Закрывающая скобка
                        btnCloseBracket.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "]";
                        break;
                    case Key.OemOpenBrackets:
                        btnOpenBracket.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "[";
                        break;
                    case Key.OemQuotes: //апостроф
                        btnApostrophe.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            userInput.Text += "'";
                        break;
                    case Key.LWin:
                        btnLeftWin.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.RWin:
                        btnRightWin.Style = this.Resources["btnOtherPress"] as Style;
                        break;
                    case Key.A:
                        btnA.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.B:
                        btnB.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.C:
                        btnC.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.D:
                        btnD.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.E:
                        btnE.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.F:
                        btnF.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.G:
                        btnG.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.H:
                        btnH.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.I:
                        btnI.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.J:
                        btnJ.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.K:
                        btnK.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.L:
                        btnL.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.M:
                        btnM.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.N:
                        btnN.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.O:
                        btnO.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.P:
                        btnP.Style = this.Resources["btnGroupGreenPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.Q:
                        btnQ.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.R:
                        btnR.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.S:
                        btnS.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.T:
                        btnT.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.U:
                        btnU.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.V:
                        btnV.Style = this.Resources["btnGroupBluePress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.W:
                        btnW.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key); 
                        break;
                    case Key.X:
                        btnX.Style = this.Resources["btnGroupYellowPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.Y:
                        btnY.Style = this.Resources["btnGroupMagentaPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    case Key.Z:
                        btnZ.Style = this.Resources["btnGroupPinkPress"] as Style;
                        if (mistake == false)
                            PrintSymbol(e.Key);
                        break;
                    default:
                        break;
                }
            }
            //подтверждаем пользователю верный ввод зеленым фоном
            if (userInput.Text.Length > 0 && userInput.Text[userInput.Text.Length - 1] == compString.Text[userInput.Text.Length - 1])
            {
                userInput.Background = new SolidColorBrush(Colors.LightGreen);
                mistake = false;
            }
            //если пользователь ошибся изменяем фон и игнорируем дальнейший ввод до устранения ошибки
            else if (userInput.Text.Length > 0)
            {
                userInput.Background = new SolidColorBrush(Colors.LightPink);
                if (mistake == false)
                    (DataContext as Source).Fails++;
                mistake = true;
            }
            //при успешном вводе всего набора символов оставлем результаты и открываем настройки для нового теста
            if(userInput.Text.Length > 0 && userInput.Text.Length == compString.Text.Length)
            {
                MessageBox.Show("Тест успешно завершён");
                timer.Stop();
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
                sliderDifficult.IsEnabled = true;
                registre.IsEnabled = true;
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //возвращаем прежний стиль отжатой кнопке
            switch (e.Key)
            {
                case Key.Back:
                    btnBack.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.Enter:
                    btnEnter.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.Space:
                    btnSpace.Style = this.Resources["btnSpaceStyle"] as Style;
                    break;
                case Key.D0:
                    btn0.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.D1:
                    btn1.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.D2:
                    btn2.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.D3:
                    btn3.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.D4:
                    btn4.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.D5:
                    btn5.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.D6:
                    btn6.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.D7:
                    btn7.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.D8:
                    btn8.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.D9:
                    btn9.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.NumPad0:
                    btn0.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.NumPad1:
                    btn1.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.NumPad2:
                    btn2.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.NumPad3:
                    btn3.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.NumPad4:
                    btn4.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.NumPad5:
                    btn5.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.NumPad6:
                    btn6.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.NumPad7:
                    btn7.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.NumPad8:
                    btn8.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.NumPad9:
                    btn9.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.LeftShift:
                    btnLeftShift.Style = this.Resources["btnOther"] as Style;
                    IsUpper = false;
                    if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock) == true)
                        IsUpper = !IsUpper;
                    break;
                    break;
                case Key.RightShift:
                    btnRightShift.Style = this.Resources["btnOther"] as Style;
                    IsUpper = false;
                    if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock) == true)
                        IsUpper = !IsUpper;
                    break;
                    break;
                case Key.LeftCtrl:
                    btnLeftCtrl.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.RightCtrl:
                    btnRightCtrl.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.System:
                    btnLeftAlt.Style = btnRightAlt.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.Oem1: //точка с запятой
                    btnSemicolon.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.OemPlus: //Равно
                    btnPlus.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.OemComma:
                    btnComma.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.OemMinus:
                    btnMinus.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.OemPeriod: //Точка
                    btnPoint.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.OemQuestion: //слеш
                    btnSlash.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.Oem3: //Тильда
                    btnTilda.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.Oem5: //Бекслеш
                    btnBackSlash.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.Oem6: //Закрывающая скобка
                    btnCloseBracket.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.OemOpenBrackets:
                    btnOpenBracket.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.OemQuotes: //апостроф
                    btnApostrophe.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.LWin:
                    btnLeftWin.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.RWin:
                    btnRightWin.Style = this.Resources["btnOther"] as Style;
                    break;
                case Key.A:
                    btnA.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.B:
                    btnB.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.C:
                    btnC.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.D:
                    btnD.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.E:
                    btnE.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.F:
                    btnF.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.G:
                    btnG.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.H:
                    btnH.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.I:
                    btnI.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.J:
                    btnJ.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.K:
                    btnK.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.L:
                    btnL.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.M:
                    btnM.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.N:
                    btnN.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.O:
                    btnO.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.P:
                    btnP.Style = this.Resources["btnGroupGreen"] as Style;
                    break;
                case Key.Q:
                    btnQ.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                case Key.R:
                    btnR.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.S:
                    btnS.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.T:
                    btnT.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.U:
                    btnU.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.V:
                    btnV.Style = this.Resources["btnGroupBlue"] as Style;
                    break;
                case Key.W:
                    btnW.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.X:
                    btnX.Style = this.Resources["btnGroupYellow"] as Style;
                    break;
                case Key.Y:
                    btnY.Style = this.Resources["btnGroupMagenta"] as Style;
                    break;
                case Key.Z:
                    btnZ.Style = this.Resources["btnGroupPink"] as Style;
                    break;
                default:
                    break;
            }
        }

        private void sliderDifficult_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            difficult.Text = sliderDifficult.Value.ToString();
        }

        //метод для печати большой/маленькой буквы
        void PrintSymbol(Key k)
        {
            if (IsUpper)
                userInput.Text += k.ToString();
            else
                userInput.Text += k.ToString().ToLower();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            (DataContext as Source).Speed = userInput.Text.Length / seconds;
        }
    }
}
