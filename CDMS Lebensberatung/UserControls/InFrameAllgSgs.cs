﻿using System.Configuration;
using CDMS_Lebensberatung.cs;

namespace CDMS_Lebensberatung.AddSpecific;

public partial class AllgSgs : UserControl
{
    public AllgSgs()
    {
        InitializeComponent();
    }

    private void AllgSgs_Load(object sender, EventArgs e)
    {
        dropErwerb.DataSource = Lists.AllgSgsErwerb;
        dropStand.DataSource = Lists.AllgSgsLebensstand;
        dropStaat.DataSource = Lists.AllgSgsStaat;
    }

    private void OnButtonSave(object sender, EventArgs e)
    {
        Dictionaries.AllgSgs.Clear();

        Dictionaries.AllgSgs.Add("Jahr", Dictionaries.Allgemein["Jahr"]);
        Dictionaries.AllgSgs.Add("Alter", Dictionaries.Allgemein["E1"][..2]);

        ReadInput.FromTextBox(this, Dictionaries.AllgSgs);
        ReadInput.FromDropDown(this, Dictionaries.AllgSgs);
        ReadInput.FromNumberBox(this, Dictionaries.AllgSgs);

        var result = ReadInput.LetUserVerify(Dictionaries.AllgSgs);
        if (result != DialogResult.OK) return;

        Sql database = new(ConfigurationManager.AppSettings.Get("ConnectionString"));
        database.Connect();
        database.InsertStringDict("Allgemeine Schwangerschaft", Dictionaries.AllgSgs);
        database.Disconnect();
    }
}