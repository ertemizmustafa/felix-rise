using Felix.Data.Core.UnitOfWork;
using Felix.Scheduler.Enums;
using Felix.Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Felix.Scheduler.Process
{
    public class SoapProcess : BaseProcess
    {

        private HttpWebRequest _httpWebRequest;

        public SoapProcess(string connectionString, string transactionId, string jobId, string jobName) : base(connectionString, transactionId, jobId, jobName)
        {
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(_connectionString);
            _httpWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
            _httpWebRequest.Accept = "text/xml";
            _httpWebRequest.Method = "POST";

        }
        public override async Task<dynamic> FetchAsync<T>(ResourceProcessModel model)
        {
            dynamic result;
            _httpWebRequest.Headers.Add("SOAPAction", model.Service);
            //Parametere replace
            model.Command = ApplyCommandParametersToSoapCommand(model.Command, model.CommandParameters);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(model.Command);
            XNamespace xmlNameSpace = model.ServiceOutput;

            using var stream = await _httpWebRequest.GetRequestStreamAsync();
            soapEnvelopeXml.Save(stream);

            WebResponse response = await _httpWebRequest.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            var docs = XDocument.Load(responseStream);

            result = docs.Descendants(xmlNameSpace + model.CommandOutputName).Select(x => x.Elements().Where(i => i.HasElements).Select(i => new Tuple<string, object>(i.Name.LocalName, i.Value)).ToDictionary(dic => dic.Item1, dic => dic.Item2)).ToList();


            return result;
        }

        public override async Task<dynamic> SendAsync(TargetProcessModel model)
        {
            dynamic result;
            _httpWebRequest.Headers.Add("SOAPAction", model.Service);
            //Parametere replace
            model.Command = ApplyCommandParametersToSoapCommand(model.Command, model.CommandParameters);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(model.Command);
            XNamespace xmlNameSpace = model.ServiceOutput;
            _httpWebRequest.ReadWriteTimeout = 100000;

            using var stream = await _httpWebRequest.GetRequestStreamAsync();
            soapEnvelopeXml.Save(stream);

            WebResponse response = await _httpWebRequest.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            var docs = XDocument.Load(responseStream);

            result = docs.Descendants(xmlNameSpace + model.CommandOutputName).Select(x => x.Elements().Where(i => i.HasElements).Select(i => new Tuple<string, object>(i.Name.LocalName, i.Value)).ToDictionary(dic => dic.Item1, dic => dic.Item2)).ToList();


            return result;
        }
    }
}
