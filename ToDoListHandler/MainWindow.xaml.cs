﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using ToDoListHandler.Classes;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string callingProcess = "";

        private readonly Data_userInterface data_UserInterface;
        private readonly Data_handler data_Handler;

        private readonly string XamlPath;

        private readonly DataTable dataTable = new DataTable();

        public MainWindow()
        {
            try
            {
                callingProcess = Environment.GetCommandLineArgs().Last().Replace("caller=", "");
            }
            catch { }

            InitializeComponent();

            InitializeTextBox();

            this.XamlPath = FindXamlfile();
            this.data_Handler = new Data_handler();
            //this.data_UserInterface = new Data_userInterface(dataTable, this.XamlPath);
            this.data_UserInterface = new Data_userInterface(rich_textbox_1, this.XamlPath);

            label_todoList_title.Content = callingProcess;

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            data_UserInterface.UpdateTextBox();
        }

        private void SaveGrid()
        {
            TodoListClass todoListClass = new TodoListClass();
            todoListClass.document = rich_textbox_1.Document;
            data_Handler.SaveXamlObject(todoListClass, this.XamlPath);
        }

        private void InitializeTextBox()
        {

        }

        private string FindXamlfile()
        {
            string callingProcessClean = callingProcess.Replace("\\", "").Replace(":", "");
            string tmpFileName = $"{callingProcessClean}.xaml";
            string res = "";

            string XamlFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\processtodo_todolists\\";

            if (!Directory.Exists(XamlFolderPath))
                Directory.CreateDirectory(XamlFolderPath);


            string[] files = Directory.GetFiles(XamlFolderPath, tmpFileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            else
            {
                try
                {
                    res = XamlFolderPath + tmpFileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return res;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveGrid();
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                App.Current.MainWindow.DragMove();
            }
            catch
            {
                //Rechte Maustaste
            }
        }

        private void Button_maximize_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                App.Current.MainWindow.WindowState = WindowState.Normal;
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
                App.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            SaveGrid();
            Environment.Exit(1);
        }

        private void Button_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void TodoList_mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveGrid();
        }
    }
}
