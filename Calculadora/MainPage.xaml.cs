using System.Data;
using System.Text.RegularExpressions;

namespace Calculadora
{
    public partial class MainPage : ContentPage
    {
        string expresion = "";

        public MainPage()
        {
            InitializeComponent();
        }

        void Num_Clicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            expresion += boton.Text;
            Mostrar.Text = expresion;
        }

        void Punto_Clicked(object sender, EventArgs e)
        {
            string[] partes = expresion.Split(new char[] { '+', '-', '*', '/' });
            string ultimoNumero = partes.Length > 0 ? partes[^1] : "";

            if (ultimoNumero.Contains(".") || ultimoNumero.EndsWith(")"))
            {
                return;
            }

            if (expresion.Length == 0 || "+-*/".Contains(expresion[expresion.Length - 1]))
            {
                expresion += "0";
            }

            expresion += ".";
            Mostrar.Text = expresion;
        }

        void Operador_Clicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            string operador = boton.Text;

            if (operador == "%")
            {
                if (expresion.Length == 0) return;

                Match match = Regex.Match(expresion, @"(\d+(\.\d+)?)$");

                if (match.Success)
                {
                    string numeroBaseStr = match.Groups[1].Value;

                    if (double.TryParse(numeroBaseStr, out double numeroBase))
                    {
                        expresion = expresion.Substring(0, expresion.Length - numeroBaseStr.Length);

                        double valorPorcentual = numeroBase / 100.0;
                        expresion += valorPorcentual.ToString();

                        Mostrar.Text = expresion;
                        return;
                    }
                }
                return;
            }

            operador = operador == "×" ? "*" : operador;

            if (expresion.Length == 0)
            {
                if (operador != "-")
                    return;
            }
            else
            {
                char ultimo = expresion[expresion.Length - 1];
                if ("+-*/".Contains(ultimo))
                {
                    expresion = expresion.Substring(0, expresion.Length - 1);
                }
            }

            expresion += operador;
            Mostrar.Text = expresion;
        }

        void Parentesis_Clicked(object sender, EventArgs e)
        {
            int abiertos = expresion.Count(c => c == '(');
            int cerrados = expresion.Count(c => c == ')');
            string parentesis = abiertos > cerrados ? ")" : "(";

            if (parentesis == "(" && expresion.Length > 0)
            {
                char ultimo = expresion[expresion.Length - 1];
                if (char.IsDigit(ultimo) || ultimo == ')')
                {
                    expresion += "*";
                }
            }

            if (parentesis == ")" && expresion.Length > 0)
            {
                char ultimo = expresion[expresion.Length - 1];
                if ("+-*/(".Contains(ultimo))
                    return;
            }

            expresion += parentesis;
            Mostrar.Text = expresion;
        }

        void Limpiar_Clicked(object sender, EventArgs e)
        {
            expresion = "";
            Mostrar.Text = "0";
        }

        void Borrar_Clicked(object sender, EventArgs e)
        {
            if (expresion.Length > 0)
            {
                expresion = expresion.Substring(0, expresion.Length - 1);
                Mostrar.Text = expresion.Length > 0 ? expresion : "0";
            }
        }

        void Igual_Clicked(object sender, EventArgs e)
        {
            if (expresion.Length > 0)
            {
                char ultimo = expresion[expresion.Length - 1];
                if ("+-*/(".Contains(ultimo))
                {
                    Mostrar.Text = "Error Sintaxis";
                    return;
                }
            }

            try
            {
                string expresionFinal = expresion;
                expresionFinal = Regex.Replace(expresionFinal, @"(\d+(\.\d+)?)(\d+(\.\d+)?)", m =>
                {
                    return m.Groups[1].Value + "*" + m.Groups[3].Value;
                });

                var resultado = new DataTable().Compute(expresionFinal, null);
                string resultadoStr = resultado.ToString();

                Mostrar.Text = resultadoStr;
                expresion = resultadoStr;
            }
            catch (Exception)
            {
                Mostrar.Text = "Error";
                expresion = "";
            }
        }
    }
}