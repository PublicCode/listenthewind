using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace T2VDCM.Energizer
{
    public static class SiteDictionaryMangager
    {
        public static Dictionary<string, string> statusMapper = new Dictionary<string, string>(5){
        {"",""},
        {"Draft", "Status_1.png"},
        {"Requested", "Status_2.png"},
        {"Quoted", "Status_3.png"},
        {"Booking", "Status_4.png"},
        {"Closed", "Status_0.png"},
        {"ReQuote", "Status_2.png"},
        {"Shipping", "Status_5.png"}
        };
        public static Dictionary<string, string> workFlowMapper = new Dictionary<string, string>(){
            {"Draft", "t2v_Queue.SubmitRFQ(@Model.HeaderId);"},
            {"Requested", "t2v_Queue.SubmitQuote($('#HeaderId').val());"}
        };
        public static Dictionary<string, string> titleWorkFlowMapper = new Dictionary<string, string>(){
            {"Draft", "Submit quote"},
            {"Requested", "Quote this quote"},
            {"Quoted", "Coming work flow"},
            {"Booking", "Coming work flow"},
            {"ReQuote", "Quote this quote"},
            {"Shipping", "Shipping this quote"},
            {"Closed", "Coming work flow"},
        };

        public static Dictionary<string, string> AuthMapper = new Dictionary<string, string>(){
        {"",""},
        {"Draft", "Status_1.png"},
        {"Requested-Modify", "Status_1.png"},
        {"Requested-Reject", "Status_1.png"},
        {"Requested", "Status_2.png"},
        {"Requested-TPV", "Status_2.png"},
        {"Requested-Response", "Status_2.png"},
        {"Requested-Resubmit", "Status_2.png"},
        {"Quoted", "Status_3.png"},
        {"Claim", "Status_4.png"},
        {"Closed", "Status_5.png"},
        {"Closed-Requote", "Status_5.png"},
        };

        public static Dictionary<string, string> ClaimMapper = new Dictionary<string, string>(){
        {"",""},
        {"Draft", "Claim1.png"},
        {"Requested", "Claim2.png"},
        {"Reviewed", "Claim3.png"},
        {"PricingReviewed","Claim4.png"},
        {"PayoutRequest", "Claim5.png"},
        {"Approved", "Claim6.png"},
        {"Credit Issued", "Claim7.png"}
        };

        public static Dictionary<string, string> AuthVIRMapper = new Dictionary<string, string>(){
        {"",""},
        {"Draft", "Status_1.png"},
        {"Requested", "Status_2.png"},
        {"Approved", "Status_3.png"},
        {"Claim", "Status_4.png"},
        {"Closed", "Status_5.png"},
        };
    }
}