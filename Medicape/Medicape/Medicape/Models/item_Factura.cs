﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Medicape.Models
{
    class item_Factura
    {
        public int iditemfactura { get; set; }
        public int idfactura { get; set; }
        public string concepto { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }
        public float total { get; set; }
    }
}