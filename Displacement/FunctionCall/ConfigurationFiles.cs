#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：DESKTOP-FMADD4L
 * 命名空间：PrismFrame.FunctionCall
 * 唯一标识：bfaead35-ccef-44ad-85ad-7c92e57070ee
 * 文件名：ConfigurationFiles
 * 当前用户域：DESKTOP-FMADD4L
 * 创建者：Forever
 * 创建时间：2022/7/13 18:39:02
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>
using Displacement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Displacement.FunctionCall
{
    /// <summary>
    /// APP运行配置文件保存
    /// </summary>
    public class ConfigurationFiles : Log
    {
        /// <summary>
        /// 路径数据
        /// </summary>
        private Dictionary<string, string> _pathdata = new Dictionary<string, string>();

        public Dictionary<string, string> PathData
        {
            get { return _pathdata; }
        }

        /// <summary>
        /// 轴数据
        /// </summary>
        private Dictionary<string, string> _axisdata = new Dictionary<string, string>();

        public Dictionary<string, string> AxisData
        {
            get { return _axisdata; }
        }

        /// <summary>
        /// 程序参数数据
        /// </summary>
        private Dictionary<string, string> _parametrdata = new Dictionary<string, string>();
        public Dictionary<string, string> ParameterData
        {
            get { return _parametrdata; }
        }

        private string _xmlpath = "System.xml";
        public string XmlPath
        {
            get { return _xmlpath; }
            set { _xmlpath = value; Log.Info($"配置文件路径设置为{value}"); }
        }

        private static object _ob = new object();

        public enum Path
        {
            AtPresent
        }

        public enum Axis
        {
            SysteamPath,
            ModbusTCPiP,
            ModbusTCPport,
            PowerComName,
            PowerBaudRate,
            GatheringModule1_IP,
            GatheringModule2_IP,
            GatheringModule1_Port,
            GatheringModule2_Port, 
            Standard1, 
            Standard2, 
            Standard3, 
            Standard4, 
            Tone_up1, 
            Tone_up2, 
            Tone_up3, 
            Tone_up4, StandardTime_set, Arguments,Set_V,Set_A,Dispense232_Baudrate, Dispense232ComName,
            OneAmmeterCom, OneAmmeterBaudrate, TwoAmmeterCom, TwoAmmeterBaudrate
        }

        public enum Parameter
        {

        }


        public enum Producttype
        {
            Null
        }


        private static ConfigurationFiles _thisfiles;

        public static ConfigurationFiles thisfiles
        {
            get { return _thisfiles; }
        }


        public ConfigurationFiles()
        {
            _thisfiles = this;
            Path penum = new Path();
            foreach (var item in System.Enum.GetNames(penum.GetType()))
            {
                _pathdata.Add(item, null);
            }
            Axis aenum = new Axis();
            foreach (var item in System.Enum.GetNames(aenum.GetType()))
            {
                _axisdata.Add(item, null);
            }
            Parameter prenum = new Parameter();
            foreach (var item in System.Enum.GetNames(prenum.GetType()))
            {
                _parametrdata.Add(item, null);
            }
            if (!File.Exists("System.xml"))
            {
                initial();
            }
            ReaderPath();
        }


        /// <summary>
        /// 配置文件读取数据到Dictionary集合
        /// </summary>
        public void ReaderPath()
        {
            Monitor.Enter(_ob);
            try
            {
                XElement xe = XElement.Load(XmlPath);//加载XML文件
                foreach (var item in _pathdata.ToArray())
                {
                    var elements = from ele in xe.Elements(item.Key) select ele;
                    foreach (var ele in elements)
                    {
                        //获取路径数据
                        _pathdata[ele.Name.ToString()] = ele.Attribute("Value").Value;
                    }
                }
                IEnumerable<XElement> eleent = from element in xe.Elements(PathData["AtPresent"]) select element;
                var inta = eleent.Elements();
                foreach (var ele in inta)
                {
                    if (_axisdata.ContainsKey(ele.Name.ToString()))
                    {
                        _axisdata[ele.Name.ToString()] = ele.Attribute("Value").Value;
                    }
                    else if (_parametrdata.ContainsKey(ele.Name.ToString()))
                    {
                        _parametrdata[ele.Name.ToString()] = ele.Attribute("Value").Value;
                    }
                }
                ReflectModelProperty(MainWindowViewModel.thisModel);
            }
            finally
            {
                Monitor.Exit(_ob);
            }
        }

        /// <summary>
        /// 更改指定的参数数据
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public void AmendPath<T>(T t, string name, string value) where T : class
        {
            Type type = typeof(T);
            var fields = type.GetProperty(name);
            if (fields != null)
            {
                Monitor.Enter(_ob);
                try
                {
                    try
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(XmlPath);
                        var nodeList = xmlDoc.SelectSingleNode("Employees");//获取Employees节点的所有子节点
                        var conlist = nodeList.SelectSingleNode(PathData["AtPresent"]).ChildNodes;
                        if (_pathdata.ContainsKey(name))
                        {
                            foreach (XmlNode xn in nodeList)//遍历所有子节点
                            {
                                XmlElement xe = (XmlElement)xn;
                                if (xn.Name == name)
                                {
                                    xe.SetAttribute("Value", value);
                                    xe.SetAttribute("ModificationTime", DateTime.Now.ToString());
                                    _pathdata[xe.Name] = value;
                                    xmlDoc.Save(XmlPath);//保存。
                                    if (name == "AtPresent")
                                    {
                                        ReaderPath();
                                    }
                                }
                            }
                        }
                        else if (_axisdata.ContainsKey(name))
                        {
                            foreach (XmlNode item in conlist)
                            {
                                XmlElement xe = (XmlElement)item;
                                if (xe.Name == name)
                                {
                                    xe.SetAttribute("Value", value);
                                    xe.SetAttribute("ModificationTime", DateTime.Now.ToString());
                                    _axisdata[xe.Name] = value;
                                }
                            }
                        }
                        else if (_parametrdata.ContainsKey(name))
                        {
                            foreach (XmlNode item in conlist)
                            {
                                XmlElement xe = (XmlElement)item;
                                if (xe.Name == name)
                                {
                                    xe.SetAttribute("Value", value);
                                    xe.SetAttribute("ModificationTime", DateTime.Now.ToString());
                                    _parametrdata[xe.Name] = value;
                                }
                            }
                        }
                        ReflectModelProperty(MainWindowViewModel.thisModel, name, value);
                        xmlDoc.Save(XmlPath);//保存。
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                    }
                }
                finally
                {
                    Monitor.Exit(_ob);
                }
            }
            else
            {
                Log.Error($"ClassMainWindowViewModel in inexistence {name}！");
            }
        }

        /// <summary>
        /// 配置文件创建
        /// </summary>
        public void initial()
        {
            var xmldoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmldecl);
            var xmlelem = xmldoc.CreateElement("", "Employees", "");
            Type StatusType = typeof(Producttype);
            //枚举所有的项
            Array YesOrNolist = Enum.GetValues(StatusType);
            foreach (var y in YesOrNolist)
            {
                var xmlelem1 = xmldoc.CreateElement(y.ToString());
                xmlelem.AppendChild(xmlelem1);
            }
            xmldoc.AppendChild(xmlelem);
            try
            {
                foreach (var item in _pathdata)
                {
                    XmlNode root = xmldoc.SelectSingleNode("Employees");//查找<Employees>
                    XmlElement xe1 = xmldoc.CreateElement(item.Key);//创建一个<Node>节点
                    xe1.SetAttribute("Value", item.Value);//设置该节点genre属性
                    xe1.SetAttribute("ModificationTime", DateTime.Now.ToString());//设置该节点ISBN属性
                    root.AppendChild(xe1);//添加到<Employees>节点中
                }
                foreach (var typeitem in YesOrNolist)
                {
                    foreach (var item in _axisdata)
                    {
                        XmlNode root = xmldoc.SelectSingleNode("Employees");//查找<Employees>
                        XmlNode son = root.SelectSingleNode(typeitem.ToString());//查找<Employees>
                        XmlElement xe1 = xmldoc.CreateElement(item.Key);//创建一个<Node>节点
                        xe1.SetAttribute("Value", item.Value);//设置该节点genre属性
                        xe1.SetAttribute("ModificationTime", DateTime.Now.ToString());//设置该节点ISBN属性
                        son.AppendChild(xe1);//添加到<Employees>节点中
                    }
                    foreach (var item in _parametrdata)
                    {
                        XmlNode root = xmldoc.SelectSingleNode("Employees");//查找<Employees>
                        XmlNode son = root.SelectSingleNode(typeitem.ToString());//查找<Employees>
                        XmlElement xe1 = xmldoc.CreateElement(item.Key);//创建一个<Node>节点
                        xe1.SetAttribute("Value", item.Value);//设置该节点genre属性
                        xe1.SetAttribute("ModificationTime", DateTime.Now.ToString());//设置该节点ISBN属性
                        son.AppendChild(xe1);//添加到<Employees>节点中
                    }
                }
                //保存创建好的XML文档
                xmldoc.Save(XmlPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 读取所有集合数据反射到属性
        /// </summary>
        /// <param name="thismodel">数据模型</param>
        public void ReflectModelProperty<T>(T thismodel) where T : class
        {
            Type StatusPath = typeof(Path);
            Type StatusAxis = typeof(Axis);
            Type StatusPara = typeof(Parameter);
            //反射枚举所有的项
            Array enumname = Enum.GetValues(StatusPath);
            try
            {
                foreach (var item in enumname)
                {
                    string key = null;
                    System.Reflection.PropertyInfo propertyInfoName = (thismodel.GetType()).GetProperty(item.ToString());//反射获取数据模型中的属性名称
                    PathData.TryGetValue(item.ToString(), out key);
                    propertyInfoName.SetValue(thismodel, key, null);
                }
                enumname = Enum.GetValues(StatusAxis);
                foreach (var item in enumname)
                {
                    string key = null;
                    System.Reflection.PropertyInfo propertyInfoName = (thismodel.GetType()).GetProperty(item.ToString());
                    AxisData.TryGetValue(item.ToString(), out key);
                    propertyInfoName.SetValue(thismodel, key, null);
                }
                enumname = Enum.GetValues(StatusPara);
                foreach (var item in enumname)
                {
                    string key = null;
                    System.Reflection.PropertyInfo propertyInfoName = (thismodel.GetType()).GetProperty(item.ToString());
                    ParameterData.TryGetValue(item.ToString(), out key);
                    propertyInfoName.SetValue(thismodel, key, null);
                }
            }
            catch (Exception ex)
            {
                Log.LogVision(ex.Message, Log.State.Error);
            }
        }


        /// <summary>
        /// 根据属性名称反射设置数据模型中的值
        /// </summary>
        /// <param name="thismodel">数据模型</param>
        /// <param name="property">属性名称</param>
        /// <param name="value">值</param>
        public void ReflectModelProperty<T>(T thismodel, string property, object value) where T : class
        {
            //Type StatusPath = typeof(Path);
            //Type StatusAxis = typeof(Axis);
            //Type StatusPara = typeof(Parameter);
            //Array enumname = Enum.GetValues(StatusPath);
            string key = null;
            Type type = typeof(T);
            var fields = type.GetProperty(property);
            if (fields != null)
            {
                System.Reflection.PropertyInfo propertyInfoName = (thismodel.GetType()).GetProperty(property);//反射获取数据模型中的属性名称
                if (Enum.IsDefined(typeof(Path), property))
                {
                    PathData.TryGetValue(property, out key);
                }
                else if (Enum.IsDefined(typeof(Axis), property))
                {
                    AxisData.TryGetValue(property, out key);
                }
                else if (Enum.IsDefined(typeof(Parameter), property))
                {
                    ParameterData.TryGetValue(property, out key);
                }
                propertyInfoName.SetValue(thismodel, key, null);
            }
            else
            {
                Log.Error($"ClassMainWindowViewModel in inexistence {property}！");
            }
        }
    }
}
