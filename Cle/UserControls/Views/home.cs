﻿using Cle.Classes;
using static System.DateTime;

namespace Cle.UserControls.Views;

public partial class home : UserControl
{
    public home()
    {
        InitializeComponent();
    }

    private void OnFrameLoad(object sender, EventArgs e)
    {
        var name = Environment.UserName;
        if (name == "ubak") name = "Ute";
        var wochentag = Lists.Wochentage[(int)Now.DayOfWeek];

        labelHallo.Text = $"Hallo {name}!";

        labelDatum.Text = $"{wochentag}, {Now.Day}.{Now.Month}.{Now.Year}";

    }
}