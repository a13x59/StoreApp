﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace StoreApp.Models
{
    public class StatisticsViewModel
    {
        [DisplayName("Наименование продукта")]
        public string productName { set; get; }

        [DisplayName("Доступное количество")]
        public int count { set; get; }
    }
}