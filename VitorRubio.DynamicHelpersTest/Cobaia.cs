using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitorRubio.DynamicHelpersTest
{
    /// <summary>
    /// o grupo de classes Cobaia e OutraCobaia servirá para fazermos 
    /// testes e comparações com tipos estáticos tradicionais e sua 
    /// conversão para dynamic, serialização para Json e duck typing
    /// </summary>
    public class Cobaia
    {
        public int UmIntQualquer { get; set; }
        public int? UmIntNulavelQualquer { get; set; }

        public double UmDoubleQualquer { get; set; }
        public double? UmDoubleNulavelQualquer { get; set; }

        public DateTime UmDateTimeQualquer { get; set; }
        public DateTime? UmDateTimeNulavelQualquer { get; set; }

        public string UmaStringQualquer { get; set; }

        public List<OutraCobaia> ListaObjetos { get; set; }

        public OutraCobaia ObjetoAssociado { get; set; }
    }

    public class OutraCobaia
    {
        public int Id { get; set; }
        public int Descricao { get; set; }
    }
}
