using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Calculadora
{
    public partial class MainPage : ContentPage
    {
        string expresion = "";

        public MainPage()
        {
            InitializeComponent();
        }
        
        private void Porcentaje_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(expresion)) return;
            int i = expresion.Length - 1;

            while (i >= 0 && (char.IsDigit(expresion[i]) || expresion[i] == '.')) i--;
            string num = expresion.Substring(i + 1);

            if (num == "" || !double.TryParse(num, out double n)) return;
            n = n / 100.0;

            expresion = expresion.Substring(0, i + 1) + n.ToString();
            Mostrar.Text = expresion;
        }

        private void Num_Clicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            expresion += boton.Text;
            Mostrar.Text = expresion;
        }

        private void Operador_Clicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            string operador = boton.Text == "×" ? "*" : boton.Text;

            if (expresion.Length == 0 && operador != "-")
                return;

            char ultimo = expresion[expresion.Length - 1];
            if ("+-*/%".Contains(ultimo))
                return;

            expresion += operador;
            Mostrar.Text = expresion;
        }

        private void Punto_Clicked(object sender, EventArgs e)
        {
            expresion += ".";
            Mostrar.Text = expresion;
        }

        private void Parentesis_Clicked(object sender, EventArgs e)
        {
            int abiertos = expresion.Split('(').Length - 1;
            int cerrados = expresion.Split(')').Length - 1;
            string parentesis = abiertos > cerrados ? ")" : "(";

            if (parentesis == "(" && expresion.Length > 0)
            {
                char ultimo = expresion[expresion.Length - 1];
                if (char.IsDigit(ultimo) || ultimo == ')')
                {
                    expresion += "*";
                }
            }

            expresion += parentesis;
            Mostrar.Text = expresion;
        }

        private void Borrar_Clicked(object sender, EventArgs e)
        {
            if (expresion.Length > 0)
                expresion = expresion.Substring(0, expresion.Length - 1);
            Mostrar.Text = expresion.Length > 0 ? expresion : "0";
        }

        private void Limpiar_Clicked(object sender, EventArgs e)
        {
            expresion = "";
            Mostrar.Text = "0";
        }

        private void Igual_Clicked(object sender, EventArgs e)
        {
            try
            {
                var resultado = new System.Data.DataTable().Compute(expresion, null);
                Mostrar.Text = resultado.ToString();
                expresion = resultado.ToString();
            }
            catch
            {
                Mostrar.Text = "Error";
                expresion = "";
            }
        }
    }
}