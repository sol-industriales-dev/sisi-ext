using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_ShapeBase
    {
        public int id { get; set; }
        public int shapeTypeID { get; set; }
        public tblMural_ShapeTypes shapeType { get; set; }
        public int refID { get; set; }
        public string name { get; set; }
        public bool draggable { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal scaleX { get; set; }
        public decimal scaleY { get; set; }
        public decimal offsetX { get; set; }
        public decimal offsetY { get; set; }
        public bool isFill { get; set; }
        public bool isStroke { get; set; }
        public string fill { get; set; }
    }
}
