﻿namespace shop_hubLaps.Models.Vnpay
{
    public class PaymentInformationModel
    {
        public int OrderId { get; set; } 

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string OrderDescription { get; set; }

        public string OrderType { get; set; }

    }

}

