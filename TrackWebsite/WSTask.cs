using StateInternetConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace TrackWebsite
{
    static class MyGetAtribute
    {
        public static HtmlElement PrevSibling(this HtmlElement element)
        {
            HtmlElement prevS = null;

            if (element == null || element.Parent == null) return null;

            HtmlElementCollection simblings = element.Parent.Children;
            for (int i = 1; i < simblings.Count; i++)
            {
                if (simblings[i] == element)
                {
                    prevS = simblings[i - 1];
                    break;
                }
            }
            return prevS;
        }

        public static string GetAttributeEx(this HtmlElement element, string atributeName)
        {
            string a = element.OuterHtml.ToLower(), A = element.OuterHtml;
            atributeName = " " + atributeName.ToLower();
            int startTag = a.IndexOf("<"), endTag = a.IndexOf(">");
            int atributeEnd = a.Substring(startTag).IndexOf(atributeName) + startTag + atributeName.Length;
            if (atributeEnd > endTag || atributeEnd - startTag - atributeName.Length < 0 || a[atributeEnd] != '=')
            {
                return "";
            }
            atributeEnd++;
            a = a.Substring(atributeEnd, endTag - atributeEnd);
            A = A.Substring(atributeEnd, endTag - atributeEnd);

            int valueEnd;
            if (a[0] == '"')
            {
                a = a.Substring(1);
                A = A.Substring(1);
                valueEnd = a.IndexOf('"');
            }
            else
            {
                valueEnd = a.IndexOf(' ');
            }

            string value;
            if (valueEnd < 0)
            {
                value = A;
            }
            else
            {
                value = A.Remove(valueEnd);
            }
            return value;
        }
    }

    public class WSTask
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }
        public Element Container { get; set; }
        public List<Pathway> Pathways { get; set; } = new List<Pathway>(1);

        #region Constructors
        public WSTask() { }

        public WSTask(string name, string link, Pathway result)
        {
            Name = name;
            Link = link;
            Pathways.Add(result);
        }

        public WSTask(string name, string link, List<Pathway> results)
        {
            Name = name;
            Link = link;
            Pathways = results;
        }

        public WSTask(string name, string link, Element container, Pathway result)
        {
            Name = name;
            Link = link;
            Container = container;
            Pathways.Add(result);
        }

        public WSTask(string name, string link, Element container, List<Pathway> results)
        {
            Name = name;
            Link = link;
            Container = container;
            Pathways = results;
        }
        #endregion

        #region Nested Structs And Classes
        public enum HowSearch { ExactMatch, Contains, NotContains, StartsWith, EndsWith };
        public enum EDistance { First, Last = -1, Any = -2, Num2 = 2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, Num10 }
        public enum ELocation { In, Around, Below, Higher }
        public enum DataType { Auto, Get, Check }

        public class Pathway
        {
            public int Id { get; set; }

            public List<Element> elements { get; set; } = new List<Element>(1);
            public bool isGetDataFromContainer { get; set; } = true;

            public Pathway() { }

            public Pathway(Data data)
            {
                elements.Add(new Element(null, data));
                isGetDataFromContainer = true;
            }

            public Pathway(List<Data> data)
            {
                elements.Add(new Element(null, data));
                isGetDataFromContainer = true;
            }

            public Pathway(Element element, bool isGetDataFromContainer = true)
            {
                elements.Add(element);
                this.isGetDataFromContainer = isGetDataFromContainer;
            }

            public Pathway(List<Element> elements, bool isGetDataFromContainer = true)
            {
                this.elements = elements;
                this.isGetDataFromContainer = isGetDataFromContainer;
            }

            public string GetResults(string separator)
            {
                string[] r = elements.Select(x => x.GetResults(separator)).Where(x => x != null && x != "").ToArray();
                return string.Join(separator, r);
            }
        }

        public class Element
        {
            public int Id { get; set; }

            public string Tag { get; set; } = null;
            public ELocation Where { get; set; } = 0;
            public EDistance WhichOne { get; set; } = 0;
            public List<Data> Data { get; set; } = new List<Data>(1);

            public Element() { }

            public Element(string tag, EDistance whichOne = EDistance.First, ELocation where = ELocation.In)
            {
                Tag = tag;
                WhichOne = whichOne;
                Where = where;
            }

            public Element(string tag, Data data, EDistance whichOne = EDistance.First, ELocation where = ELocation.In)
            {
                Tag = tag;
                Data.Add(data);
                WhichOne = whichOne;
                Where = where;
            }

            public Element(string tag, List<Data> data, EDistance whichOne = EDistance.First, ELocation where = ELocation.In)
            {
                Tag = tag;
                Data = data;
                WhichOne = whichOne;
                Where = where;
            }

            public Element(string tag, string prop, EDistance whichOne = EDistance.First, ELocation where = ELocation.In)
            {
                Tag = tag;
                Data.Add(new Data(prop));
                WhichOne = whichOne;
                Where = where;
            }

            public Element(string tag, string prop, string value, EDistance whichOne = EDistance.First, ELocation where = ELocation.In)
            {
                Tag = tag;
                Data.Add(new Data(prop, value));
                WhichOne = whichOne;
                Where = where;
            }

            public string[] GetResults()
            {
                string[] r = Data.Select(x => (x.result.HasValue) ? x.result.Value : x.result.Error).Where(x => x != null).ToArray();
                return r;
            }

            public string GetResults(string separator)
            {
                string[] r = Data.Select(x => (x.result.HasValue) ? x.result.Value : x.result.Error).Where(x => x != null).ToArray();
                return string.Join(separator, r);
            }
        }

        public class Data
        {
            public int Id { get; set; }

            private DataType type = DataType.Auto;
            public DataType Type
            {
                get { return type; }
                set { type = value; CheckType(); }
            }
            public string Prop { get; set; } = null;
            public string Value { get; set; } = null;
            public HowSearch howSearch { get; set; } = 0;
            public Result result { get; set; } = new Result();

            public Data(DataType type = DataType.Get)
            {
                Type = type;
            }

            public Data(string value, HowSearch howS, DataType type = DataType.Check)
            {
                Value = value;
                howSearch = howS;
                Type = type;
            }

            public Data(string prop, DataType type = DataType.Get)
            {
                Prop = prop;
                Type = type;
            }

            public Data(string prop, string value, HowSearch howS = HowSearch.ExactMatch, DataType type = DataType.Check)
            {
                Value = value;
                Prop = prop;
                howSearch = howS;
                Type = type;
            }

            private void CheckType()
            {
                if (Value == null || Value == "")
                {
                    if (Type == DataType.Get)
                    {
                        Type = DataType.Auto;
                    }
                }
                else
                {
                    if (Type == DataType.Check)
                    {
                        Type = DataType.Auto;
                    }
                }
            }

            public DataType GetRealType()
            {
                if (Value == null || Value == "")
                {
                    return DataType.Get;
                }
                else
                {
                    return DataType.Check;
                }
            }
        }

        public class Result
        {
            public bool HasValue { get; set; } = false;
            public string Error { get; set; } = null;
            public string Value { get; set; } = null;

            public Result() { }

            public Result(string value, string error)
            {
                HasValue = value != null;
                Value = value;
                Error = error;
            }

            public static string ErrorContainerNotFound = "Ошибка: контейнер не найден";
            public static string ErrorElementNotFound = "Ошибка: элемент не найден";
            public static string ErrorIncorrectDataType = "Ошибка: невозможно выполнить предназначение, проверьте правильно ли указан тип данных";
            public static string ErrorPropertyNotFound = "Ошибка: свойство не найдено";
        }
        #endregion

        public bool DoWork(HtmlDocument document)
        {
            //Сбрасываем предыдущие результаты
            for (int i = 0; i < Pathways.Count; i++)
            {
                for (int j = 0; j < Pathways[i].elements.Count; j++)
                {
                    SetNull(Pathways[i].elements[j]);
                }
            }

            //Поиск контейнера
            HtmlElement cont = null;
            if (Container != null && Container.Tag != null && Container.Tag != "")
            {
                List<HtmlElement> tmp = GetElement(document, Container);
                if (tmp != null)
                {
                    cont = tmp[0];
                }
                else
                {
                    SetErrorOrFalse(Container, Result.ErrorElementNotFound);
                }
            }

            //Проходимся по всем путям
            for (int p = 0; p < Pathways.Count; p++)
            {
                //Если исходные элемент пути - это контейнер, задаём GetElement
                HtmlElement ge = null;
                if (Pathways[p].isGetDataFromContainer)
                {
                    if (cont == null)
                    {
                        for (int j = 0; j < Pathways[p].elements.Count; j++)
                        {
                            SetError(Pathways[p].elements[j], Result.ErrorContainerNotFound);
                        }
                        continue;
                    }
                    else
                    {
                        ge = cont;
                    }
                }

                // Эти переменные и цикл нужны для реализации EDistance.Any
                bool anyActive = false, Success = false;
                List<HtmlElement> anyList = new List<HtmlElement>();
                int anyStart = 0;

                int max = 1;
                for (int any = anyActive ? 1 : 0; any < max; any++)
                {
                    Success = true;
                    if (anyActive)
                    {
                        ge = anyList[any];
                    }
                    
                    // Перебираем элементы
                    for (int el = anyActive ? anyStart : 0; el < Pathways[p].elements.Count; el++)
                    {
                        Element element = Pathways[p].elements[el];

                        if (element.Tag != null && (!anyActive || el != anyStart))
                        {
                            //Поиск нужного исходного элемента / проверка на его наличие
                            List<HtmlElement> tmp = GetElement(ge == null ? document.Body : ge, element);
                            if (tmp == null)
                            {
                                SetErrorOrFalse(element, Result.ErrorElementNotFound);
                                Success = false;
                                break;
                            }
                            ge = tmp[0];

                            //Активация EDistance.Any
                            if (element.WhichOne == EDistance.Any && tmp.Count > 1 && !anyActive)
                            {
                                anyActive = true;
                                anyStart = el;
                                anyList = tmp;
                                max = anyList.Count;
                            }
                        }
                        //Поиск нужных данных / Проверка на их наличие
                        for (int j = 0; j < element.Data.Count; j++)
                        {
                            Result result = GetData(ge, element.Data[j], DataType.Get);
                            element.Data[j].result = result;
                        }
                    }

                    if (anyActive)
                    {
                        if (Success)
                        {
                            break;
                        }
                        else
                        {
                            SetErrorOrFalse(Pathways[p].elements[anyStart], Result.ErrorElementNotFound);
                            for (int e = anyStart + 1; e < Pathways[p].elements.Count; e++)
                            {
                                SetNull(Pathways[p].elements[e]);
                            }
                        }
                    }
                }
            }
            return true;
        }

        #region Add-on modules
        public static void OpenUri(WebBrowser browser, string uri)
        {
            if (uri != "") //&& ())
            {
                if (browser.Url != null && browser.Url.AbsoluteUri.EndsWith(uri))
                {
                    browser.Refresh();
                }
                else
                {
                    browser.Navigate(uri);
                }
                Application.DoEvents();
                DateTime start = DateTime.Now;
                while (browser.FindForm() != null && browser.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents(); //Выполняем другие события системы, пока страница не загрузилась
                    if ((DateTime.Now - start).TotalSeconds > 10 && browser.ReadyState == WebBrowserReadyState.Interactive)
                    {
                        browser.Stop();
                        break;
                    }
                }
            }
        }

        public static bool isInternetConnected(string site = null)
        {
            IPStatus status = InternetStatus(site);

            if (status == IPStatus.Success || status == IPStatus.TimedOut)
            {
                //Если превышен таймаут, не значит, что нет интернета
                return true;
            }
            else 
            {
                return false;
            }
        }

        public static IPStatus InternetStatus(string site = null)
        {
            IPStatus result = IPStatus.Unknown;
            //Проверяем, есть ли интернет
            InternetConnection inet = new InternetConnection();
            inet.Init();
            if (inet.IsInternetConnected)
            {
                if (site == null) return IPStatus.Success;

                //Проверяем, еслть ли доступ к сайту
                try
                {
                    //Находим имя сайта
                    int index = site.IndexOf("//");
                    if (index != -1)
                    {
                        site = site.Substring(index + 2);
                    }
                    if (site.StartsWith("www."))
                    {
                        site = site.Substring(4);
                    }
                    index = site.IndexOf("/");
                    if (index != -1)
                    {
                        site = site.Remove(index);
                    }
                    //Проверяем пинг
                    Ping p = new Ping();
                    PingReply pr = p.Send(site);
                    result = pr.Status;
                }
                catch
                {
                    result = IPStatus.NoResources;
                }
            }
            else
            {
                result = IPStatus.NoResources;
            }
            return result;
        }

        public Pathway GetPathwayById(long id)
        {
            for (int p = 0; p < Pathways.Count; p++)
            {
                if (Pathways[p].Id == id)
                {
                    return Pathways[p];
                }
            }
            return null;
        }

        public bool ReplacePathwayById(long id, Pathway path)
        {
            for (int p = 0; p < Pathways.Count; p++)
            {
                if (Pathways[p].Id == id)
                {
                    Pathways[p] = path;
                    Pathways.RemoveAll(x => x == null);
                    return true;
                }
            }
            return false;
        }

        public Element GetElementById(long id)
        {
            if (Container != null && Container.Id == id)
            {
                return Container;
            }
            for (int p = 0; p < Pathways.Count; p++)
            {
                for (int e = 0; e < Pathways[p].elements.Count; e++)
                {
                    if (Pathways[p].elements[e].Id == id)
                    {
                        return Pathways[p].elements[e];
                    }
                }
            }
            return null;
        }

        public bool ReplaceElementById(long id, Element element)
        {
            if (Container != null && Container.Id == id)
            {
                Container = element;
                return true;
            }
            for (int p = 0; p < Pathways.Count; p++)
            {
                for (int e = 0; e < Pathways[p].elements.Count; e++)
                {
                    if (Pathways[p].elements[e].Id == id)
                    {
                        Pathways[p].elements[e] = element;
                        Pathways[p].elements.RemoveAll(x => x == null);
                        return true;
                    }
                }
            }
            return false;
        }

        public Data GetDataById(long id)
        {
            if (Container != null)
            {
                for (int d = 0; d < Container.Data.Count; d++)
                {
                    if (Container.Data[d].Id == id)
                    {
                        return Container.Data[d];
                    }
                }
            }
            for (int p = 0; p < Pathways.Count; p++)
            {
                for (int e = 0; e < Pathways[p].elements.Count; e++)
                {
                    for (int d = 0; d < Pathways[p].elements[e].Data.Count; d++)
                    {
                        if (Pathways[p].elements[e].Data[d].Id == id)
                        {
                            return Pathways[p].elements[e].Data[d];
                        }
                    }
                }
            }
            return null;
        }

        public bool ReplaceDataById(long id, Data data)
        {
            if (Container != null)
            {
                for (int d = 0; d < Container.Data.Count; d++)
                {
                    if (Container.Data[d].Id == id)
                    {
                        Container.Data[d] = data;
                        Container.Data.RemoveAll(x => x == null);
                        return true;
                    }
                }
            }
            for (int p = 0; p < Pathways.Count; p++)
            {
                for (int e = 0; e < Pathways[p].elements.Count; e++)
                {
                    for (int d = 0; d < Pathways[p].elements[e].Data.Count; d++)
                    {
                        if (Pathways[p].elements[e].Data[d].Id == id)
                        {
                            Pathways[p].elements[e].Data[d] = data;
                            Pathways[p].elements[e].Data.RemoveAll(x => x == null);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SetError(Element element, string error)
        {
            for (int i = 0; i < element.Data.Count; i++)
            {
                Result r = new Result();
                r.Error = error;
                element.Data[i].result = r;
            }
        }

        private void SetErrorOrFalse(Element element, string error)
        {
            SetNull(element);

            for (int i = 0; i < element.Data.Count; i++)
            {
                Result r = new Result();
                if (element.Data[i].GetRealType() == DataType.Check && element.Data[i].Type == DataType.Get)
                {
                    r.HasValue = true;
                    r.Value = "False";
                }
                else
                {
                    r.Error = error;
                }
                element.Data[i].result = r;
            }
        }

        private void SetNull(Element element)
        {
            for (int i = 0; i < element.Data.Count; i++)
            {
                element.Data[i].result = new Result();
            }
        }
        #endregion

        #region Get & Compare Data on web page
        public string GetResults(string separator)
        {
            string[] r = Pathways.Select(x => x.GetResults(separator)).Where(x => x != null && x != "").ToArray();
            return string.Join(separator, r);
        }

        public static List<HtmlElement> GetElement(HtmlDocument startingPoint, Element element)
        {
            if (startingPoint == null) return null;

            return GetElement(startingPoint.Body, element);
        }

        public static List<HtmlElement> GetElement(HtmlElement startingPoint, Element element)
        {
            if (startingPoint == null) return null;

            List<HtmlElement> match = new List<HtmlElement>();
            switch (element.Where)
            {
                case ELocation.In:
                {
                    match = startingPoint.GetElementsByTagName(element.Tag).Cast<HtmlElement>().ToList();
                    break;
                }
                case ELocation.Around:
                {
                    match.Add(startingPoint);
                    bool canDown = true, canUp = true;
                    for (int i = 0; canDown || canUp; i++)
                    {
                        HtmlElement nextElement;
                        if (canDown && (i % 2 == 0 || !canUp))
                        {
                            nextElement = match.Last().NextSibling;
                            canDown = nextElement != null;
                        }
                        else
                        {
                            nextElement = match.Last().PrevSibling();
                            canUp = nextElement != null;
                        }
                        if (nextElement != null) match.Add(nextElement);
                    }
                    match.RemoveAt(0);
                    break;
                }
                case ELocation.Below:
                { 
                    HtmlElement nextElement = startingPoint.NextSibling;
                    while (nextElement != null)
                    {
                        match.Add(nextElement);
                        nextElement = match.Last().NextSibling;
                    }
                    break;
                }
                case ELocation.Higher:
                {
                    HtmlElement nextElement = startingPoint.PrevSibling();
                    while (nextElement != null)
                    {
                        match.Add(nextElement);
                        nextElement = match.Last().PrevSibling();
                    }
                    break;
                }
                default:
                    match = null;
                    break;
            }

            //Находим всё, что подходят под описание
            for (int i = 0; i < match.Count; i++)
            {
                if (!CheckHtmlElement(match[i], element))
                {
                    match.RemoveAt(i);
                    i--;
                }
            }
            if (match.Count == 0) return null;

            //Находим нужные в списке тех, которые подходят под описание
            match = GetElementByDistance(match, element.WhichOne);

            return match;
        }

        public static bool CheckHtmlElement(HtmlElement target, Element element)
        {
            bool isMatch = true;

            if (target.TagName.ToLower() != element.Tag.ToLower()) return false;

            foreach (Data data in element.Data)
            {
                if (data.GetRealType() != DataType.Check)
                {
                    continue;
                }

                if (data.Prop == "" || data.Prop == null)
                {
                    Result t = GetData(target, data, DataType.Check);
                    if (!t.HasValue || !Convert.ToBoolean(t.Value))
                    {
                        isMatch = false;
                        break;
                    }
                }
                else
                {
                    if (!IsValueFit(target.GetAttributeEx(data.Prop), data.Value, data.howSearch))
                    {
                        isMatch = false;
                        break;
                    }
                }
            }
            return isMatch;
        }

        public static List<HtmlElement> GetElementByDistance(List<HtmlElement> elements, EDistance whichOne)
        {
            if (elements.Count == 0) return null;

            List<HtmlElement> r = new List<HtmlElement>(1);
            switch (whichOne)
            {
                case EDistance.First:
                    r.Add(elements.First());
                    break;
                case EDistance.Last:
                    r.Add(elements.Last());
                    break;
                case EDistance.Any:
                    r = elements;
                    break;
                default:
                    int n = (int)whichOne - 1;
                    if (n > 0 && elements.Count > n)
                    {
                        r.Add(elements[n]);
                    }
                    else
                    {
                        return null;
                    }
                    break;
            }
            return r;
        }

        public static bool IsValueFit(string value, string CompareWith, HowSearch howCompare)
        {
            value = value.ToLower();
            CompareWith = CompareWith.ToLower();
            switch (howCompare)
            {
                case HowSearch.ExactMatch:
                    return value == CompareWith;
                case HowSearch.Contains:
                    return value.Contains(CompareWith);
                case HowSearch.NotContains:
                    return !value.Contains(CompareWith);
                case HowSearch.StartsWith:
                    return value.StartsWith(CompareWith);
                case HowSearch.EndsWith:
                    return value.EndsWith(CompareWith);
            }
            return false;
        }

        public static Result GetData(HtmlElement element, Data data, DataType type)
        {
            Result r = new Result();
            if (!(data.GetRealType() == type || (type == DataType.Get && data.Type == type)))
            {
                //Несоответствие типа запроса и предоставленных данных
                if (data.Type != DataType.Auto && data.Type != data.GetRealType())
                {
                    r.Error = Result.ErrorIncorrectDataType;
                }
                return r;
            }

            string str;
            if (data.Prop == "" || data.Prop == null)
            {
                str = element.OuterText;
            }
            else
            {
                str = element.GetAttributeEx(data.Prop);
            }

            if (str == "" || str == null)
            {
                r.Error = Result.ErrorPropertyNotFound;
                return r;
            }

            r.HasValue = true;
            if (data.Value == "" || data.Value == null)
            {
                //if Get
                r.Value = str;
            }
            else
            {
                //if Check
                bool isFit = IsValueFit(str, data.Value, data.howSearch);
                r.Value = isFit.ToString();
            }
            return r;
        }
        #endregion
    }
}
