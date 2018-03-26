using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PerfMon
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class MainWindow : Window
    {
        // 디렉토리 변수
        private String sDirectoryPath;
        // 시간 변수
        private Int32 iProgressMax;
        
        // 경과시간 선언
        TimeSpan AddTimeSpan = new TimeSpan(0, 0, 0);

        // 백그라운드워커 생성
        private BackgroundWorker thread = new BackgroundWorker();

        // 프로세스명
        String ProcessName = "";

        public MainWindow()
        {
            // Mutex를 이용하여 중복 실행 방지하기
            bool NewCheck;
            
            // 최초 설정한 프로세스에 초기 소유권을 부여하고 이 후 동일한 프로세스에는 소유권을 주지 않기
            Mutex m = new Mutex(true, "PerfMon", out NewCheck);

            if (NewCheck)
            {
                InitializeComponent();
            }
            else
            {
                System.Windows.MessageBox.Show("이미 실행중입니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                Close();
            }
        }

        // OnInitialized 메서드 오버라이딩하기
        protected override void OnInitialized(System.EventArgs e)
        {
            // 이벤트 발생 시 초기화
            base.OnInitialized(e);



            // 진행률 전송 여부
            thread.WorkerReportsProgress = true;

            // 작업 취소 여부
            thread.WorkerSupportsCancellation = true;

            // 작업 쓰레드 
            thread.DoWork += new DoWorkEventHandler(thread_DoWork);

            // 진행률 변경
            thread.ProgressChanged += new ProgressChangedEventHandler(thread_ProgressChanged);

            // 작업 완료
            thread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(thread_RunWorkerCompleted);
        }

        // SA1 라디오버튼 Checked 이벤트 처리기
        private void SA1RadioBurron_Checked(object sender, RoutedEventArgs e)
        {
            ProcessName = "SuddenAttack";
        }

        // SA2 라디오버튼 Checked 이벤트 처리기
        private void SA2RadioBurron_Checked(object sender, RoutedEventArgs e)
        {
            ProcessName = "SA2";
        }

        // 시간 카운팅 단추 이벤트 최대치 처리 (최소 1, 최대 999)
        private void RepeatUpButton_Click(object sender, RoutedEventArgs e)
        {
            int iNowTime = Int32.Parse(this.SetTime.Text);
            if (iNowTime < 999)
            {
                iNowTime++;
            }
            this.SetTime.Text = iNowTime.ToString();
        }

        // 시간 카운팅 단추 이벤트 최소치 처리 (최소 1, 최대 999)
        private void RepeatDownButton_Click(object sender, RoutedEventArgs e)
        {
            int iNowTime = Int32.Parse(this.SetTime.Text);
            if (iNowTime > 1)
            {
                iNowTime--;
            }
            this.SetTime.Text = iNowTime.ToString();
        }

        // 시간 텍스트박스에 숫자만 입력하기 (최소 1, 최대 999)
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // 입력된 문자를 바이트 크기로 인코딩 하기
            int length = Encoding.Default.GetBytes(this.SetTime.Text).Length;

            // 정규 표현식으로 0 ~ 9 (숫자)를 제외한 문자를 1회 이상 지정
            Regex regex = new Regex("[^0-9]+");

            // 시간 텍스트박스에 입력된 자리수 및 키보드로 입력받은 키 체크
            if (length == 3 | regex.IsMatch(e.Text))
            {
                // 숫자가 아닐 경우 출력되는 팝업창 띄우지 않기
                this.NoNumberAllert.IsOpen = false;

                // 숫자가 오버될 경우 출력되는 팝업창 띄우지 않기
                this.OverNumberAllert.IsOpen = false;

                // 텍스트 박스에 입력하기
                e.Handled = true;

            }
            //System.Windows.MessageBox.Show(this.SetTime.Text);
            // 키보드로 입력받은 키가 숫자가 아니면 경고 팝업 출력
            if (regex.IsMatch(e.Text) == true)
            {
                this.OverNumberAllert.IsOpen = false;
                this.NoNumberAllert.IsOpen = true;
            }
            // 숫자키를 입력 받았으나 시간 텍스트박스에 입력된 값이 3자리를 넘어가면 경고 팝업 출력
            if (length == 3)
            {
                if (regex.IsMatch(e.Text) == false)
                {
                    this.OverNumberAllert.IsOpen = true;
                }
            }
        }

        // 폴더 확인창에서 경로를 선택 하였을 경우
        private void DirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            // 폴더 선택창 생성
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 폴더 선택창에서 선택한 폴더 경로를 입력하기
                this.Directory.Text = dialog.SelectedPath;
            }
        }


        // 경로 텍스트박스에 경로를 입력 하였을 경우
        private void Keybord_Preview_Text_Input(object sender, TextCompositionEventArgs e)
        {
            // 입력된 문자를 바이트 크기로 인코딩 하기
            int length = Encoding.Default.GetBytes(this.SetTime.Text).Length;

            // 정규 표현식으로 [/], [:], [*], [?], ["], [<], [>], [|]인 문자를 1회 이상 지정
            Regex regex = new Regex("[/:*?\"<>|]+");

            // 지정된 문자가 입력되면
            if (regex.IsMatch(e.Text))
            {
                this.FolderAllert.IsOpen = true;
                e.Handled = true;
            }
            else
            {

            }
        }

        // 프로그램 시작 시 UI 변경하기
        private void StartUI()
        {
            // 버튼 텍스트 변경
            this.StartButton.Content = "중지";

            // 버튼 비활성화
            this.SetTime.IsEnabled = false;
            this.RepeatUpButton.IsEnabled = false;
            this.RepeatDownButton.IsEnabled = false;
            this.DirectoryButton.IsEnabled = false;
            this.EndButton.IsEnabled = false;
            this.Directory.IsEnabled = false;

            // 프로그레스바 불투명하게 변경
            this.Progress.Opacity = 1;
            // 프로그레스바 값 불투명하게 변경
            this.ProgressValue.Opacity = 1;

            // 아이콘 변경
            System.Drawing.Icon Icon = PerfMon.Properties.Resources.Sign_Record_Icon;
            MemoryStream iconStream = new MemoryStream();
            Icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame NewIcon = BitmapFrame.Create(iconStream);
            this.Icon = NewIcon;

            // 이미지 변경
            System.Drawing.Bitmap Image = PerfMon.Properties.Resources.Sign_Record_Image;
            MemoryStream imgStream = new MemoryStream();
            Image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
            imgStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame NewImage = BitmapFrame.Create(imgStream);
            MainImage.Source = NewImage;

            // 타이머 초기화
            AddTimeSpan = TimeSpan.Zero;
        }

        // 프로그램 종료 시 UI 변경하기
        private void EndUI()
        {
            // 버튼 텍스트 변경
            this.StartButton.Content = "시작";

            // 버튼 활성화
            this.SetTime.IsEnabled = true;
            this.RepeatUpButton.IsEnabled = true;
            this.RepeatDownButton.IsEnabled = true;
            this.DirectoryButton.IsEnabled = true;
            this.EndButton.IsEnabled = true;
            this.Directory.IsEnabled = true;

            // 프로그레스바 투명하게 변경
            this.Progress.Opacity = 0;
            // 프로그레스바 값 투명하게 변경
            this.ProgressValue.Opacity = 0;

            // 아이콘 변경
            System.Drawing.Icon Icon = PerfMon.Properties.Resources.Sign_Icon;
            MemoryStream iconStream = new MemoryStream();
            Icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame NewIcon = BitmapFrame.Create(iconStream);
            this.Icon = NewIcon;

            // 이미지 변경
            System.Drawing.Bitmap Image = PerfMon.Properties.Resources.Sign_Image;
            MemoryStream imgStream = new MemoryStream();
            Image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
            imgStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame NewImage = BitmapFrame.Create(imgStream);
            MainImage.Source = NewImage;
        }

        // 설정한 경로의 드라이브 체크하기
        private bool DriveCheck()
        {
            // 결과값 변수
            Int32 iDriveCheck = 0;
            
            // 드라이브 정보 생성
            DriveInfo[] DriveCheck = DriveInfo.GetDrives();

            // 드라이브 정보 할당
            foreach (DriveInfo d in DriveCheck)
            {
                // 입력한 드라이브가 실제로 있는지 비교
                iDriveCheck = String.Compare(d.Name, 0, this.Directory.Text, 0, 3, true);

                if (iDriveCheck == 0)
                {
                    // 해당 드라이브가 준비중인지 비교
                    if (d.IsReady)
                    {
                        //해당 드라이브에 선택한 크기만큼 사용 가능한 용량이 있는지 확인
                        if (d.AvailableFreeSpace >= 100000000)
                        {
                            break;
                        }
                        else
                        {
                            iDriveCheck = 2;
                            break;
                        }
                    }
                    else
                    {
                        iDriveCheck = 1;
                        break;
                    }
                }
            }

            switch (iDriveCheck)
            {
                // 드라이브가 실제로 있으며 준비중이며 사용 가능한 용량이면
                case (0):
                    {
                        return true;
                    }
               
                // 드라이브가 준비되어 있지 않으면
                case (1):
                    {
                        System.Windows.MessageBox.Show("드라이브가 준비되지 않았습니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                        return false;
                    }
                
                // 드라이브에 사용 가능한 용량이 적으면
                case (2):
                    {
                        System.Windows.MessageBox.Show("드라이브 용량이 적습니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                        return false;
                    }
                
                // 그외
                default:
                    {
                        System.Windows.MessageBox.Show("지정된 드라이브가 없거나 잘못 입력하였습니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                        return false;
                    }
            }
        }

        // 서든어택이 실행되고 있는지 체크하기
        private Int32 ProcessCheck()
        {
            // 결과값 변수
            Int32 iProcessCheck = 0;

            // 특정 프로세스 대입            
            Process[] process = Process.GetProcessesByName(ProcessName);

            // 현재 실행중인 모든 프로레스 취합
            Process currentProcess = Process.GetCurrentProcess();
            
            // 취합한 프로세스 수만큼 돌리기
            foreach (Process p in process)
            {
                // 취합한 프로세스중에 특정 프로세스가 없으면
                if (p.Id != currentProcess.Id)
                {
                    iProcessCheck += 1;
                }
                // 있으면
                else
                {
                    iProcessCheck += 0;
                }
            }

            return iProcessCheck;
        }

        // 백그라운드 워커가 종료되었을 경우
        private void thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EndUI();

            String sResult = "작업이 완료되었습니다!";

            // 취소 되었을 경우
            if (e.Cancelled)
            {
                sResult = "작업이 중지되었습니다!";
            }

            System.Windows.MessageBox.Show(sResult, "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
        }

        // 진행 상태가 변경되었을 경우
        private void thread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            // 백그라운드 진행률 대입
            Int32 iValue = e.ProgressPercentage;


            // 변경 값으로 갱신
            this.Progress.Value = e.ProgressPercentage;
            this.ProgressValue.Content = iValue * 100 / iProgressMax + "%";


            // 경과 시간 측정하기
            AddTimeSpan = TimeSpan.FromSeconds(iValue);
            this.Timer.Content = AddTimeSpan;
        }

        // 백그라운드워커가 작업중일 경우
        private void thread_DoWork(object sender, DoWorkEventArgs e)
        { 
            // 로그 파일명 변수
            String sFileName;


            // 디렉토리 정보 생성
            DirectoryInfo DirectoryPathCheck = new DirectoryInfo(sDirectoryPath);

            // 선택한 디렉토리가 없으면
            if (DirectoryPathCheck.Exists == false)
            {
                // 폴더 생성
                DirectoryPathCheck.Create();
            }


            // 총 CPU 사용률 퍼포먼스 카운터 생성
            PerformanceCounter PerfCounterTotalCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            // 선택한 프로세스의 CPU 사용률 퍼포먼스 카운터 생성
            PerformanceCounter PerfCounterProcessCPU = new PerformanceCounter("Process", "% Processor Time", ProcessName);
            // 선택한 프로세스가 사용중인 메모리량 퍼포먼스 카운터 생성
            PerformanceCounter PerfCounterMemory = new PerformanceCounter("Process", "Private Bytes", ProcessName);

            // 현재 시간으로 파일명 지정 및 csv 확장자 지정하기
            if (ProcessName == "SuddenAttack")
            {
                sFileName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + @"_SA1.csv";
            }
            else
            {
                sFileName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + @"_SA2.csv";
            }

            // 설정한 이름으로 로그파일을 설정한 폴더에 생성
            StreamWriter LogFileWriter = new StreamWriter(@sDirectoryPath + sFileName, true);


            // 생성한 로그파일에 첫행(항목명) 삽입
            LogFileWriter.WriteLine("Sec, Process _% Processor Time_" + ProcessName + ", Process_Private Bytes_" + ProcessName + ", Processor_% Processor_Total");


            // 백그라운드워커 실행
            BackgroundWorker worker = sender as BackgroundWorker;

            
            // 설정한 시간(수)만큼 돌리기
            for (Int32 i = 0; i <= iProgressMax; i++)
            {
                // CancelAsync() 메서드가 호출되었다면
                if (worker.CancellationPending == true)
                {
                    // 이벤트 정지
                    e.Cancel = true;
                    break;
                }
                // 그게 아니면
                else
                {
                    // 1초동안 쓰레드 대기하기
                    System.Threading.Thread.Sleep(1000);

                    // 생성한 로그파일에 행 단위로 퍼포먼스 카운트 값 삽입
                    LogFileWriter.WriteLine("{0}, {1}, {2}, {3}", System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), PerfCounterProcessCPU.NextValue().ToString(), PerfCounterMemory.NextValue().ToString(), PerfCounterTotalCPU.NextValue().ToString());
                                    
                    // 진행률 변경 값 전송
                    worker.ReportProgress(i);
                }
            }

            // 로그 파일 닫기
            LogFileWriter.Close();
        }

        // 시작 버튼을 눌렀을 경우
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 드라이브에도 이상이 없고
            if (DriveCheck())
            {
                // 해당 프로세스가 실행 중이라면
                if (ProcessCheck() == 1)
                {
                    // 지정한 시간 초 단위로 환산
                    iProgressMax = Int32.Parse(this.SetTime.Text) * 60;

                    // 지정한 시간을 프로그레스바 맥스에 대입
                    this.Progress.Maximum = iProgressMax;
                    
                    // 지정한 경로에 오늘날자의 하위 폴더 추가
                    sDirectoryPath = this.Directory.Text + @"\" + System.DateTime.Now.ToString("yyyyMMdd") + @"\";

                    // 쓰레드가 비동기 작업중이면
                    if (thread.IsBusy)
                    {
                        // UI 변경하기
                        EndUI();
                        // 백그라운드워커 취소하기
                        thread.CancelAsync();
                    }
                    // 그게아니면
                    else
                    {
                        // UI 변경하기
                        StartUI();
                        
                        // 백그라운드워커 실행하기
                        thread.RunWorkerAsync();
                    }
                }
                // 해당 프로세스가 실행중이 아니라면
                else if (ProcessCheck() == 0)
                {
                    if (ProcessName == "SuddenAttack")
                    {
                        System.Windows.MessageBox.Show("서든어택을 실행하세요!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("서든어택2를 실행하세요!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                    }
                }
                // 그것도 아니라면
                else
                {
                    if (ProcessName == "SuddenAttack")
                    {
                        System.Windows.MessageBox.Show("서든어택이 중복으로 실행되고 있습니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("서든어택2가 중복으로 실행되고 있습니다!", "PerfMon", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                    }
                }
            }
        }

        // 닫기 버튼 누를 경우
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            // 프로그램 닫기
            this.Close();
        }
    }
}