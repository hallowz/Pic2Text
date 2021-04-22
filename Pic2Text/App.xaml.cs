using Pic2Text.Data;
using Pic2Text.Models;
using Xamarin.Forms;
using System;
using System.IO;
using System.Collections.Generic;

namespace Pic2Text
{
    public partial class App : Application
    {
        static P2TDatabase database;
        public static List<P2T> history;

        // Create the database connection as a singleton.
        public static P2TDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new P2TDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            history = new List<P2T>();
            MainPage = new AppShell();
        }

        public static void addP2T(P2T p2t)
        {
            history.Add(p2t);
        }

        public static int getHistorySize()
        {
            return history.Count;
        }

        public static List<P2T> getHistory()
        {
            return history;
        }

        public static P2T getP2T(int i)
        {
            if(i < history.Count)
            {
                return history[i];
            }
            else
            {
                return history[history.Count - 1];
            }
            
        }

        public static void setP2T(P2T p, int i)
        {
            history[i] = p;
        }

        public static void setHistory(List<P2T> _history)
        {
            history = _history;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
