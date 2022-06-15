﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using ManchkinCore.Implementation;
using ManchkinCore.Interfaces;

namespace ManchkinGame.DialogWindows;

public partial class DoblePriceSell : Window
{
    private IManchkin _manchkin;
    private List<IStuff> _variants;
    public DoblePriceSell()
    {
        _manchkin = App.Current.Resources["MANCHKIN"] as IManchkin;
        _variants = _manchkin.SmallStuffs;
        _variants.AddRange(_manchkin.HugeStuffs);
        InitializeComponent();
        ShowStuffButton.Click += ShowStuffButtonClick;
        CancelButton.Click += CancelButtonClick;
        StuffComboBox.Loaded += StuffComboBoxLoaded;
        SellButton.Click += SellButtonClick;
    }

    private void SellButtonClick(object sender, RoutedEventArgs e)
    {
        if(StuffComboBox.Text == "")
            UserMessage.CreateNotChosenItemMessage("шмотку для продажи");
        else
        {
            var variant = _variants.FirstOrDefault(vari => vari.TextRepresentation == StuffComboBox.Text);
            var price = _manchkin.SellByDoublePrice(variant);
            App.Current.Resources["PRICE"] = price;
            Close();
        }
    }

    private void StuffComboBoxLoaded(object sender, RoutedEventArgs e)
    {
        foreach (var variant in _variants)
            StuffComboBox.Items.Add(variant.TextRepresentation);
    }

    private void ShowStuffButtonClick(object sender, RoutedEventArgs e)
    {
        if(StuffComboBox.Text == "")
            UserMessage.CreateNotChosenItemMessage("шмотку для просмотра");
        else
        {
            var variant = _variants.FirstOrDefault(vari => vari.TextRepresentation == StuffComboBox.Text);
            ShowStuff(variant);
        }
    }
    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        App.Current.Resources["PRICE"] = 0;
        Close();
    }
    
    private void ShowStuff(IStuff stuff)
    {
        if(stuff == null)
            UserMessage.CreateEmptyStuffMessage();
        else
        {
            Application.Current.Resources["STUFF"] = stuff;
            Application.Current.Resources["STUFF_TYPE"] = stuff switch
            {
                Armor => "броник",
                Shoes => "обувка",
                Weapon => "оружие",
                Hat => "головняк",
                _ => "просто шмотка"
            };
            DialogWindow.Show(new StuffWindow(), this);
        }
    }
}