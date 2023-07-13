using Displacement.FunctionCall;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using static Displacement.FunctionCall.Geometry;

namespace Displacement.ViewModels
{
    public partial class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        private string _runtest;

        public string RunText
        {
            get { return _runtest; }
            set { _runtest = value; RaisePropertyChanged(); }
        }


        public struct BounceTest
        {
            public double OneReturn { get; set; }
            public double TwoReturn { get; set; }
            public double ThreeReturn { get; set; }
            public double FourReturn { get; set; }
        }
        private string _systeampath;

        public string SysteamPath
        {
            get { return _systeampath; }
            set { _systeampath = value; }
        }

        private string _modbustcpip;
        /// <summary>
        /// modbus——IP地址
        /// </summary>
        public string ModbusTCPiP
        {
            get { return _modbustcpip; }
            set { _modbustcpip = value; }
        }

        private string _modbustcpport;
        /// <summary>
        /// modbus——端口号
        /// </summary>
        public string ModbusTCPport
        {
            get { return _modbustcpport; }
            set { _modbustcpport = value; }
        }

        private string _powercomname;
        /// <summary>
        /// 电源Com口名称
        /// </summary>
        public string PowerComName
        {
            get { return _powercomname; }
            set { _powercomname = value; }
        }

        private string _powerbaudrate;
        /// <summary>
        /// 电源波特率
        /// </summary>
        public string PowerBaudRate
        {
            get { return _powerbaudrate; }
            set { _powerbaudrate = value; }
        }

        private string _gatheringmodbule1_ip;
        /// <summary>
        /// 采集模块一 ip地址
        /// </summary>
        public string GatheringModule1_IP
        {
            get { return _gatheringmodbule1_ip; }
            set { _gatheringmodbule1_ip = value; }
        }

        private string _gatheringmodbule2_ip;
        /// <summary>
        /// 采集模块二 ip地址
        /// </summary>
        public string GatheringModule2_IP
        {
            get { return _gatheringmodbule2_ip; }
            set { _gatheringmodbule2_ip = value; }
        }

        private string _gatheringmodbule1_port;
        /// <summary>
        /// 采集模块一 端口号
        /// </summary>
        public string GatheringModule1_Port
        {
            get { return _gatheringmodbule1_port; }
            set { _gatheringmodbule1_port = value; }
        }

        private string _gatheringmodbule2_port;
        /// <summary>
        /// 采集模块二 端口号
        /// </summary>
        public string GatheringModule2_Port
        {
            get { return _gatheringmodbule2_port; }
            set { _gatheringmodbule2_port = value; }
        }

        private string _dispense232_comnme;
        /// <summary>
        /// 点胶机Com端
        /// </summary>
        public string Dispense232ComName
        {
            get { return _dispense232_comnme; }
            set { _dispense232_comnme = value; }
        }

        private string _dispense232_baudrate;
        /// <summary>
        /// 点胶机波特率
        /// </summary>
        public string Dispense232_Baudrate
        {
            get { return _dispense232_baudrate; }
            set { _dispense232_baudrate = value; }
        }

        private float _OilValue = 0;
        /// <summary>
        /// 注油量
        /// </summary>
        public float OilValue
        {
            get { return _OilValue; }
            set { _OilValue = (float)Math.Round(value, 4); RaisePropertyChanged(); }
        }

        private float _Oilspeed = 0;
        /// <summary>
        /// 注油速度
        /// </summary>
        public float OilSpeed
        {
            get { return _Oilspeed; }
            set { _Oilspeed = (float)Math.Round(value, 4); RaisePropertyChanged(); }
        }


        #region 曲线参数
        private string _standard1;
        /// <summary>
        /// 串动基准值一
        /// </summary>
        public string Standard1
        {
            get { return _standard1; }
            set { _standard1 = value; }
        }

        private string _standard2;
        /// <summary>
        /// 串动基准值二
        /// </summary>
        public string Standard2
        {
            get { return _standard2; }
            set { _standard2 = value; }
        }

        private string _standard3;
        /// <summary>
        /// 串动基准值三
        /// </summary>
        public string Standard3
        {
            get { return _standard3; }
            set { _standard3 = value; }
        }

        private string _standard4;
        /// <summary>
        /// 串动基准值四
        /// </summary>
        public string Standard4
        {
            get { return _standard4; }
            set { _standard4 = value; }
        }

        private string _tne_up1;
        /// <summary>
        /// 串动增益值一
        /// </summary>
        public string Tone_up1
        {
            get { return _tne_up1; }
            set { _tne_up1 = value; }
        }

        private string _tne_up2;
        /// <summary>
        /// 串动增益值二
        /// </summary>
        public string Tone_up2
        {
            get { return _tne_up2; }
            set { _tne_up2 = value; }
        }

        private string _tne_up3;
        /// <summary>
        /// 串动增益值三
        /// </summary>
        public string Tone_up3
        {
            get { return _tne_up3; }
            set { _tne_up3 = value; }
        }

        private string _tne_up4;
        /// <summary>
        /// 串动增益值四
        /// </summary>
        public string Tone_up4
        {
            get { return _tne_up4; }
            set { _tne_up4 = value; }
        }

        private string _standardtime_set;
        /// <summary>
        /// 串动采样间隔
        /// </summary>
        public string StandardTime_set
        {
            get { return _standardtime_set; }
            set { _standardtime_set = value; }
        }

        #endregion
        private string _atpresent;
        public string AtPresent
        {
            get { return _atpresent; }
            set { _atpresent = value; }
        }

        #region 串动左工位图表


        private ObservableCollection<Point2D> _serializationoneadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 串动数据一
        /// </summary>
        public ObservableCollection<Point2D> SerializationOneAdSampling
        {
            get { return _serializationoneadsampling; }
            set { _serializationoneadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationtwoadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 串动数据二
        /// </summary>
        public ObservableCollection<Point2D> SerializationTwoAdSampling
        {
            get { return _serializationtwoadsampling; }
            set { _serializationtwoadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationonepeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动一波峰
        /// </summary>
        public ObservableCollection<Point2D> SerializationOnePeak
        {
            get { return _serializationonepeak; }
            set { _serializationonepeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationonegrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动一波谷
        /// </summary>
        public ObservableCollection<Point2D> SerializationOneGrain
        {
            get { return _serializationonegrain; }
            set { _serializationonegrain = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationtwopeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动二波峰
        /// </summary>
        public ObservableCollection<Point2D> SerializationTwoPeak
        {
            get { return _serializationtwopeak; }
            set { _serializationtwopeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationtwograin = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动二波谷
        /// </summary>
        public ObservableCollection<Point2D> SerializationTwoGrain
        {
            get { return _serializationtwograin; }
            set { _serializationtwograin = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 串动右工位图表
        private ObservableCollection<Point2D> _serializationthreeadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 串动数据三
        /// </summary>
        public ObservableCollection<Point2D> SerializationThreeAdSampling
        {
            get { return _serializationthreeadsampling; }
            set { _serializationthreeadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationfouradsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 串动数据四
        /// </summary>
        public ObservableCollection<Point2D> SerializationFourAdSampling
        {
            get { return _serializationfouradsampling; }
            set { _serializationfouradsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationthreepeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动三波峰
        /// </summary>
        public ObservableCollection<Point2D> SerializationThreePeak
        {
            get { return _serializationthreepeak; }
            set { _serializationthreepeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationthreegrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动三波谷
        /// </summary>
        public ObservableCollection<Point2D> SerializationThreeGrain
        {
            get { return _serializationthreegrain; }
            set { _serializationthreegrain = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationfourpeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动四波峰
        /// </summary>
        public ObservableCollection<Point2D> SerializationFourPeak
        {
            get { return _serializationfourpeak; }
            set { _serializationfourpeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _serializationfourgrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 串动四波谷
        /// </summary>
        public ObservableCollection<Point2D> SerializationFourGrain
        {
            get { return _serializationfourgrain; }
            set { _serializationfourgrain = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 轴跳左工位图表

        private double _oneDifferenceValue;
        /// <summary>
        /// 左工位轴跳动差值
        /// </summary>
        public double OneDifferenceValue
        {
            get { return _oneDifferenceValue; }
            set { _oneDifferenceValue = value; RaisePropertyChanged(); }
        }

        private double _twoDifferenceValue;
        /// <summary>
        /// 左工位蜗杆跳到差值
        /// </summary>
        public double TwoDifferenceValue
        {
            get { return _twoDifferenceValue; }
            set { _twoDifferenceValue = value; RaisePropertyChanged(); }
        }



        private ObservableCollection<Point2D> _bounceoneadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 轴跳数据一
        /// </summary>
        public ObservableCollection<Point2D> BounceOneAdSampling
        {
            get { return _bounceoneadsampling; }
            set { _bounceoneadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncetwoadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 轴跳数据二
        /// </summary>
        public ObservableCollection<Point2D> BounceTwoAdSampling
        {
            get { return _bouncetwoadsampling; }
            set { _bouncetwoadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bounceonepeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳一波峰
        /// </summary>
        public ObservableCollection<Point2D> BounceOnePeak
        {
            get { return _bounceonepeak; }
            set { _bounceonepeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bounceonegrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳一波谷
        /// </summary>
        public ObservableCollection<Point2D> BounceOneGrain
        {
            get { return _bounceonegrain; }
            set { _bounceonegrain = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncetwopeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳二波峰
        /// </summary>
        public ObservableCollection<Point2D> BounceTwoPeak
        {
            get { return _bouncetwopeak; }
            set { _bouncetwopeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncetwograin = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳二波谷
        /// </summary>
        public ObservableCollection<Point2D> BounceTwoGrain
        {
            get { return _bouncetwograin; }
            set { _bouncetwograin = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 轴跳右工位图表

        private double _threeDifferenceValue;
        /// <summary>
        /// 右工位轴跳动差值
        /// </summary>
        public double ThreeDifferenceValue
        {
            get { return _threeDifferenceValue; }
            set { _threeDifferenceValue = value; RaisePropertyChanged(); }
        }

        private double _fourDifferenceValue;
        /// <summary>
        /// 右工位蜗杆跳动差值
        /// </summary>
        public double FourDifferenceValue
        {
            get { return _fourDifferenceValue; }
            set { _fourDifferenceValue = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncethreeadsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 轴跳数据三
        /// </summary>
        public ObservableCollection<Point2D> BounceThreeAdSampling
        {
            get { return _bouncethreeadsampling; }
            set { _bouncethreeadsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncefouradsampling = new ObservableCollection<Point2D>();

        /// <summary>
        /// 轴跳数据四
        /// </summary>
        public ObservableCollection<Point2D> BounceFourAdSampling
        {
            get { return _bouncefouradsampling; }
            set { _bouncefouradsampling = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncethreepeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳三波峰
        /// </summary>
        public ObservableCollection<Point2D> BounceThreePeak
        {
            get { return _bouncethreepeak; }
            set { _bouncethreepeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncethreegrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳三波谷
        /// </summary>
        public ObservableCollection<Point2D> BounceThreeGrain
        {
            get { return _bouncethreegrain; }
            set { _bouncethreegrain = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncefourpeak = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳四波峰
        /// </summary>
        public ObservableCollection<Point2D> BounceFourPeak
        {
            get { return _bouncefourpeak; }
            set { _bouncefourpeak = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Point2D> _bouncefourgrain = new ObservableCollection<Point2D>();
        /// <summary>
        /// 轴跳四波谷
        /// </summary>
        public ObservableCollection<Point2D> BounceFourGrain
        {
            get { return _bouncefourgrain; }
            set { _bouncefourgrain = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 程控电源
        private double _voltage;
        /// <summary>
        /// 手动输出电压值
        /// </summary>
        public double Voltage
        {
            get { return _voltage; }
            set { _voltage = value; RaisePropertyChanged(); }
        }

        private double _electricity;

        /// <summary>
        /// 手动输出电流值
        /// </summary>
        public double Electricity
        {
            get { return _electricity; }
            set { _electricity = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 参数保存

        private ObservableCollection<Parameter> _argumentsdata = new ObservableCollection<Parameter>();

        /// <summary>
        /// 轴参数设置UI集合
        /// </summary>
        public ObservableCollection<Parameter> ArgumentsData
        {
            get { return _argumentsdata; }
            set { _argumentsdata = value; RaisePropertyChanged(); }
        }

        private int _parameterindexes;
        /// <summary>
        /// 参数型号选择ComboBox
        /// </summary>
        public int ParameterIndexes
        {
            get { return _parameterindexes; }
            set { _parameterindexes = value; RaisePropertyChanged(); }
        }

        private List<ComboxList> _parameterNameList = new List<ComboxList>();
        /// <summary>
        /// 参数型号选择集合
        /// </summary>
        public List<ComboxList> ParameterNameList
        {
            get { return _parameterNameList; }
            set { _parameterNameList = value; RaisePropertyChanged(); }
        }

        private string _arguments;

        public string Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        public struct ComboxList
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }

        private string _set_v;
        /// <summary>
        /// 自动输出电压
        /// </summary>
        public string Set_V
        {
            get { return _set_v; }
            set { _set_v = value; }
        }

        private string _set_a;
        /// <summary>
        /// 自动输出电流
        /// </summary>
        public string Set_A
        {
            get { return _set_a; }
            set { _set_a = value; }
        }

        #endregion

        #region 实时图
        private double[] doubles = new double[8];

        public double[] Doubles
        {
            get { return doubles; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    doubles[i] = Math.Round(value[i], 4);
                }
                doubles = value; RaisePropertyChanged();
            }
        }

        private ObservableCollection<TemperatureChart> _bounceOneAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 轴动一
        /// </summary>
        public ObservableCollection<TemperatureChart> BounceOneAdSamplingAuto
        {
            get { return _bounceOneAdSamplingAuto; }
            set { _bounceOneAdSamplingAuto = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<TemperatureChart> _bounceTwoAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 蜗杆一
        /// </summary>
        public ObservableCollection<TemperatureChart> BounceTwoAdSamplingAuto
        {
            get { return _bounceTwoAdSamplingAuto; }
            set { _bounceTwoAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TemperatureChart> _bounceThreeAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 轴动二
        /// </summary>
        public ObservableCollection<TemperatureChart> BounceThreeAdSamplingAuto
        {
            get { return _bounceThreeAdSamplingAuto; }
            set { _bounceThreeAdSamplingAuto = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<TemperatureChart> _bounceFourAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 蜗杆二
        /// </summary>
        public ObservableCollection<TemperatureChart> BounceFourAdSamplingAuto
        {
            get { return _bounceFourAdSamplingAuto; }
            set { _bounceFourAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TemperatureChart> _serializationOneAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 串动一
        /// </summary>
        public ObservableCollection<TemperatureChart> SerializationOneAdSamplingAuto
        {
            get { return _serializationOneAdSamplingAuto; }
            set { _serializationOneAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TemperatureChart> _serializationTwoAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 串动二
        /// </summary>
        public ObservableCollection<TemperatureChart> SerializationTwoAdSamplingAuto
        {
            get { return _serializationTwoAdSamplingAuto; }
            set { _serializationTwoAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TemperatureChart> _pressureOneAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 压力一
        /// </summary>
        public ObservableCollection<TemperatureChart> PressureOneAdSamplingAuto
        {
            get { return _pressureOneAdSamplingAuto; }
            set { _pressureOneAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TemperatureChart> _pressureTwoAdSamplingAuto = new ObservableCollection<TemperatureChart>();

        /// <summary>
        /// 压力二
        /// </summary>
        public ObservableCollection<TemperatureChart> PressureTwoAdSamplingAuto
        {
            get { return _pressureTwoAdSamplingAuto; }
            set { _pressureTwoAdSamplingAuto = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 自动运行表
        private string _auto4ct;

        public string Auto4CT
        {
            get { return _auto4ct; }
            set { _auto4ct = value; RaisePropertyChanged(); }
        }

        private string _auto5ct;

        public string Auto5CT
        {
            get { return _auto5ct; }
            set { _auto5ct = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<No5> _no5TestResult = new ObservableCollection<No5>();
        /// <summary>
        /// 工位5测试结果
        /// </summary>
        public ObservableCollection<No5> No5TestResult
        {
            get { return _no5TestResult; }
            set { _no5TestResult = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<No4> _no4TestResult = new ObservableCollection<No4>();
        /// <summary>
        /// 工位4测试结果
        /// </summary>
        public ObservableCollection<No4> No4TestResult
        {
            get { return _no4TestResult; }
            set { _no4TestResult = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 自动运行线程
        private Thread[] _auot_threads = new Thread[6];

        public Thread[] Auto_Threads
        {
            get { return _auot_threads; }
            set { _auot_threads = value; }
        }

        #endregion

        #region 数据库

        private ObservableCollection<Result> _inquireresult = new ObservableCollection<Result>();
        /// <summary>
        /// 查询
        /// </summary>
        public ObservableCollection<Result> InquireResult
        {
            get { return _inquireresult; }
            set { _inquireresult = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 生产信息
        private int total_production = 0;
        /// <summary>
        /// 生产总数
        /// </summary>
        public int Total_Production
        {
            get { return total_production; }
            set { total_production = value; RaisePropertyChanged(); }
        }

        private int ng_number = 0;
        /// <summary>
        /// NG数
        /// </summary>
        public int NG_number
        {
            get { return ng_number; }
            set { ng_number = value; RaisePropertyChanged(); }
        }

        private float yield = 0;
        /// <summary>
        /// 良率
        /// </summary>
        public float Yield
        {
            get { return yield; }
            set { yield = (float)Math.Round(value, 2); RaisePropertyChanged(); }
        }
        private float ct_s = 0;
        /// <summary>
        /// CT
        /// </summary>
        public float Ct_S
        {
            get { return ct_s; }
            set { ct_s = (float)Math.Round(value, 2); ; RaisePropertyChanged(); }
        }
        private float _utilization = 0;
        /// <summary>
        /// 稼动率
        /// </summary>
        public float Utilization
        {
            get { return _utilization; }
            set { _utilization = (float)Math.Round(value, 2); ; RaisePropertyChanged(); }
        }

        private float total_running_time = 0;
        /// <summary>
        /// 总运行时间
        /// </summary>
        public float Total_Running_Time
        {
            get { return total_running_time; }
            set { total_running_time = (float)Math.Round(value, 2); ; RaisePropertyChanged(); }
        }

        private float normal_duration = 0;
        /// <summary>
        /// 正常运行时间
        /// </summary>
        public float Normal_Duration
        {
            get { return normal_duration; }
            set { normal_duration = (float)Math.Round(value, 2); ; RaisePropertyChanged(); }
        }

        private float _bnormal_duration = 0;
        /// <summary>
        /// 异常运行时间
        /// </summary>
        public float Bnormal_Duration
        {
            get { return _bnormal_duration; }
            set { _bnormal_duration = (float)Math.Round(value, 2); ; RaisePropertyChanged(); }
        }

        #endregion

        #region 电流表
        public OneAmmeter oneammeters { get; set; }
        public TwoAmmeter twoammeters { get; set; }

        private float _oneelectric;
        /// <summary>
        /// 电流值一
        /// </summary>
        public float OneElectric
        {
            get { return _oneelectric; }
            set { _oneelectric = value; RaisePropertyChanged(); }
        }

        private float _twoelectric;
        /// <summary>
        /// 电流值二
        /// </summary>
        public float TwoElectric
        {
            get { return _twoelectric; }
            set { _twoelectric = value; RaisePropertyChanged(); }
        }

        private string _oneammetercom;
        /// <summary>
        /// 电流表一Com口
        /// </summary>
        public string OneAmmeterCom
        {
            get { return _oneammetercom; }
            set { _oneammetercom = value; }
        }
        /// <summary>
        /// 电流表一波特率
        /// </summary>
        private string _oneammeterbaudrate;

        public string OneAmmeterBaudrate
        {
            get { return _oneammeterbaudrate; }
            set { _oneammeterbaudrate = value; }
        }


        private string _twoammetercom;
        /// <summary>
        /// 电流表二Com口
        /// </summary>
        public string TwoAmmeterCom
        {
            get { return _twoammetercom; }
            set { _twoammetercom = value; }
        }

        private string _twoammeterbaudrate;
        /// <summary>
        /// 电流表二波特率
        /// </summary>
        public string TwoAmmeterBaudrate
        {
            get { return _twoammeterbaudrate; }
            set { _twoammeterbaudrate = value; }
        }

        #endregion
    }

    public class Points
    {
        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public class TemperatureChart : BindableBase
    {
        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; RaisePropertyChanged(); }
        }

        private double _value;

        public double Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }
    }

    public class No5
    {
        public string Time { get; set; }

        private string _oneresult = "NG";

        public string OneResult
        {
            get { return _oneresult; }
            set
            {
                if (value == "Pass")
                {
                    OneColor = "#FF0CFF00";
                }
                else if (value == "NG")
                {
                    OneColor = "#FFFF0000";
                }
                else if (value == "NULL")
                {
                    OneColor = "#FF3771FF";
                };
                _oneresult = value;
            }
        }
        private string _tworesult = "NG";

        public string TwoResult
        {
            get { return _tworesult; }
            set
            {
                if (value == "Pass")
                {
                    TwoColor = "#FF0CFF00";
                }
                else if (value == "NG")
                {
                    TwoColor = "#FFFF0000";
                }
                else if (value == "NULL")
                {
                    TwoColor = "#FF3771FF";
                };
                _tworesult = value;
            }
        }


        public string OneColor { get; set; }
        public string TwoColor { get; set; }
        /// <summary>
        /// 一号轴动结果
        /// </summary>
        public string OneValueResult { get; set; } = "NG";

        /// <summary>
        /// 一号蜗杆动结果
        /// </summary>
        public string TwoValueResult { get; set; } = "NG";

        /// <summary>
        /// 二号轴动结果
        /// </summary>
        public string ThreeValueResult { get; set; } = "NG";

        /// <summary>
        /// 二号蜗杆动结果
        /// </summary>
        public string FourValueResult { get; set; } = "NG";

        public double OneValue { get; set; } = 0;
        public double TwoValue { get; set; } = 0;
        public double ThreeValue { get; set; } = 0;
        public double FourValue { get; set; } = 0;
        public string OneElEResult { get; set; } = "NG";
        public float OneElE { get; set; } = 0;
        public string TwoElEResult { get; set; } = "NG";

        public float TwoElE { get; set; } = 0;
    }


    public class No4
    {
        public string Time { get; set; }

        private string _oneresult = "NG";

        public string OneResult
        {
            get { return _oneresult; }
            set
            {
                if (value == "Pass")
                {
                    OneColor = "#FF0CFF00";
                }
                else if (value == "NG")
                {
                    OneColor = "#FFFF0000";
                }
                else if (value == "NULL")
                {
                    OneColor = "#FF3771FF";
                };
                _oneresult = value;
            }
        }
        private string _tworesult = "NG";

        public string TwoResult
        {
            get { return _tworesult; }
            set
            {
                if (value == "Pass")
                {
                    TwoColor = "#FF0CFF00";
                }
                else if (value == "NG")
                {
                    TwoColor = "#FFFF0000";
                }
                else if (value == "NULL")
                {
                    TwoColor = "#FF3771FF";
                };
                _tworesult = value;
            }
        }


        public string OneColor { get; set; }
        public string TwoColor { get; set; }
        /// <summary>
        /// 一号载具推位移
        /// </summary>
        public double PushOneMove { get; set; } = 0;
        /// <summary>
        /// 一号载具推结果
        /// </summary>
        public string PushOneMoveResult { get; set; } = "NG";
        /// <summary>
        /// 一号推力
        /// </summary>
        public double PushOneForce { get; set; } = 0;

        /// <summary>
        /// 一号载具推力结果
        /// </summary>
        public string PushOneForceResult { get; set; } = "NG";
        /// <summary>
        /// 二号载具推位移
        /// </summary>
        public double PushTwoMove { get; set; } = 0;

        /// <summary>
        /// 二号载具推位移结果
        /// </summary>
        public string PushTwoMoveResult { get; set; } = "NG";

        /// <summary>
        /// 二号推力
        /// </summary>
        public double PushTwoForce { get; set; } = 0;

        /// <summary>
        /// 二号载具推力结果
        /// </summary>
        public string PushTwoForceResult { get; set; } = "NG";


        /// <summary>
        /// 一号载具拉位移
        /// </summary>
        public double PullOneMove { get; set; } = 0;


        /// <summary>
        /// 一号载具拉位移结果
        /// </summary>
        public string PullOneMoveResult { get; set; } = "NG";

        /// <summary>
        /// 一号载具拉力
        /// </summary>
        public double PullOneForce { get; set; } = 0;

        /// <summary>
        /// 一号载具拉力结果
        /// </summary>
        public string PullOneForceResult { get; set; } = "NG";

        /// <summary>
        /// 二号载具拉位移
        /// </summary>
        public double PullTwoMove { get; set; } = 0;

        /// <summary>
        /// 二号载具拉位移结果
        /// </summary>
        public string PullTwoMoveResult { get; set; } = "NG";

        /// <summary>
        /// 二号载具拉力
        /// </summary>
        public double PullTwoForce { get; set; } = 0;
        /// <summary>
        /// 二号载具拉力结果
        /// </summary>
        public string PullTwoForceResult { get; set; } = "NG";


        /// <summary>
        /// 一号载具串动值
        /// </summary>
        public double One_bunch_movement { get; set; } = 0;
        /// <summary>
        /// 二号载具串动值
        /// </summary>
        public double Two_bunch_movement { get; set; } = 0;

    }


    public class Test
    {
        public No4 No4 { get; set; } = new No4();
        public No5 No5 { get; set; } = new No5();
    }

    public class Result
    {
        public string[] StringResult { get; set; } = new string[10];
        public float[] FloatResult { get; set; } = new float[32];
    }
}
