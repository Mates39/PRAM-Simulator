namespace PramSimulator_WebApp.Components.Services
{
    public class CodeService
    {
        public CodeService()
        {
            SetExamples();
        }
        public Dictionary<string, string> Codes { get; set; }
        public void SetExamples()
        {
            Codes = new Dictionary<string, string>();
            Codes.Add("Vypocet konjunkce sekvence bitu v poli", KonjunkceBitu);
        }
        private string KonjunkceBitu = "#V globální paměti na pozici S1 vzplnit početbitů\r\n#Na pozicích S2 - Sn+1 vyplnit bity\r\nS0 := 1\r\nparallelStart S1\r\nL0 := {ind}\r\nL0 := L0 + 2\r\nL1 := S[L0]\r\nif L1 == 1 goto :end\r\nS0 := 0\r\n:end\r\nhalt\r\nparallelEnd\r\nhalt";
    }
}
