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
            Codes.Add("Vypocet disjunkce sekvence bitu v poli", DisjunkceBitu);
            Codes.Add("Vypocet souctu cisel v poli", SoucetCiselVPoli);
            Codes.Add("Nalezeni minima v poli", NalezeniMinimaVPoli);
        }
        private string KonjunkceBitu = "#V globální paměti na pozici S1 vyplnit počet bitů\r\n#Na pozicích S2 - Sn+1 vyplnit bity\r\nS0 := 1\r\nparallelStart S1\r\nL0 := {ind}\r\nL0 := L0 + 2\r\nL1 := S[L0]\r\nif L1 == 1 goto :end\r\nS0 := 0\r\n:end\r\nhalt\r\nparallelEnd\r\n\r\n\r\nMEMORYINPUT\r\n(0:1)\r\n(1:5)\r\n(2:1)\r\n(3:1)\r\n(4:1)\r\n(5:0)\r\n(6:1)";
        private string DisjunkceBitu = "#V globální paměti na pozici S1 vyplnit počet bitů\r\n#Na pozicích S2 - Sn+1 vyplnit bity\r\nS0 := 0\r\nparallelStart S1\r\nL0 := {ind}\r\nL0 := L0 + 2\r\nL1 := S[L0]\r\nif L1 == 1 goto :end\r\nS0 := 1\r\n:end\r\nhalt\r\nparallelEnd\r\nhalt\r\n\r\nMEMORYINPUT\r\n(0:1)\r\n(1:5)\r\n(2:1)\r\n(3:1)\r\n(4:1)\r\n(5:0)\r\n(6:1)";
        private string SoucetCiselVPoli = "#Na pozici S0 je pocet cisel pro vypocet. Pokud je cislo liche, pridej na konec 0\r\n#Od pozice S2 vypln hodnoty\r\n#Kontrola, zda je pocet cisel sudy\r\nS1 := S0 % 2\r\n#pokud je soucet lichy prida se 1 aby byl sudy\r\nif S1 == 0 goto :calcProc\r\nS0 := S0 + 1\r\n#vypocet kolik procesoru ma byt spusteno\r\n:calcProc\r\nS0 := S0 / 2\r\nparallelStart S0\r\n#vypocet indexu, ze kterych budou nacitany hodnoty\r\nL0 := {ind}\r\nL1 := L0 * 2\r\nL1 := L1 + 2\r\nL2 := L1 + 1\r\n#nacteni hodnot\r\nL3 := S[L1]\r\nL4 := S[L2]\r\n#vymazani puvodnich hodnot\r\nS[L1] := 0\r\nS[L2] := 0\r\n#vypocet souctu\r\nL5 := L3 + L4\r\n#vypocet indexu pro ulozeni souctu\r\nL1 := L1 - L0\r\n#zapsani hodnoty do sdilene pameti\r\nS[L1] := L5\r\nhalt\r\nparallelEnd\r\n#pokud je posledni pocet spustenych procesoru 1 vypocet konci\r\nif S0 == 1 goto :end\r\ngoto :calcProc\r\n:end\r\nhalt\r\n\r\nMEMORYINPUT\r\n(0:7)\r\n(1:0)\r\n(2:1)\r\n(3:2)\r\n(4:3)\r\n(5:4)\r\n(6:5)\r\n(7:6)\r\n(8:7)\r\n(9:0)";
        private string NalezeniMinimaVPoli = "#Na pozici S0 je pocet cisel pro vypocet. Pokud je cislo liche, pridej na konec 0\r\n#Od pozice S2 vypln hodnoty\r\n#Kontrola, zda je pocet cisel sudy\r\nS1 := S0 % 2\r\n#pokud je soucet lichy prida se 1 aby byl sudy\r\nif S1 == 0 goto :calcProc\r\nS0 := S0 + 1\r\n#vypocet kolik procesoru ma byt spusteno\r\n:calcProc\r\nS0 := S0 / 2\r\nparallelStart S0\r\n#vypocet indexu, ze kterych budou nacitany hodnoty\r\nL0 := {ind}\r\nL1 := L0 * 2\r\nL1 := L1 + 2\r\nL2 := L1 + 1\r\n#nacteni hodnot\r\nL3 := S[L1]\r\nL4 := S[L2]\r\n#vymazani puvodnich hodnot\r\nS[L1] := 0\r\nS[L2] := 0\r\n#porovnani hodnot\r\nif L3 < L4 goto :L3Smaller\r\nL5 := L4\r\n:L3Smaller\r\nL5 := L3\r\n#vypocet indexu pro ulozeni souctu\r\nL1 := L1 - L0\r\n#zapsani hodnoty do sdilene pameti\r\nS[L1] := L5\r\nhalt\r\nparallelEnd\r\n#pokud je posledni pocet spustenych procesoru 1 vypocet konci\r\nif S0 == 1 goto :end\r\ngoto :calcProc\r\n:end\r\nhalt\r\n\r\nMEMORYINPUT\r\n(0:7)\r\n(1:0)\r\n(2:1)\r\n(3:2)\r\n(4:3)\r\n(5:4)\r\n(6:5)\r\n(7:6)\r\n(8:7)\r\n(9:0)";

    }
}
