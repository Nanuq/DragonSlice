﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using XRpgLibrary.CharacterClasses;
using XRpgLibrary.ItemClasses;

namespace RpgEditor
{
    public partial class FormShield : FormDetails
    {
        #region Field region



        #endregion

        #region Property region



        #endregion

        #region Constructor region

        public FormShield()
        {
            InitializeComponent();

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        #endregion

        #region Button Event Handler region

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbDetails.SelectedItem != null)
            {
                string detail = lbDetails.SelectedItem.ToString();
                string[] parts = detail.Split(',');
                string entity = parts[0].Trim();

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete " + entity + "?",
                    "Confirm",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    lbDetails.Items.RemoveAt(lbDetails.SelectedIndex);
                    itemManager.ShieldData.Remove(entity);

                    if (File.Exists(FormMain.ItemPath + @"\Shield\" + entity + ".xml"))
                    {
                        File.Delete(FormMain.ItemPath + @"\Shield\" + entity + ".xml");
                    }
                }
            }
        }

        void btnEdit_Click(object sender, EventArgs e)
        {
            if (lbDetails.SelectedItem != null)
            {
                string detail = lbDetails.SelectedItem.ToString();
                string[] parts = detail.Split(',');
                string entity = parts[0].Trim();

                ShieldData data = itemManager.ShieldData[entity];
                ShieldData newData = null;

                using (FormShieldDetails frmShieldData = new FormShieldDetails())
                {
                    frmShieldData.Shield = data;
                    frmShieldData.ShowDialog();

                    if (frmShieldData.Shield == null)
                    {
                        return;
                    }

                    if (frmShieldData.Shield.Name == entity)
                    {
                        itemManager.ShieldData[entity] = frmShieldData.Shield;
                        FillListBox();
                        return;
                    }

                    newData = frmShieldData.Shield;
                }

                DialogResult result = MessageBox.Show(
                    "Name changed. Do you want to add a new entry?",
                    "New Entry",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    return;
                }

                if (itemManager.ShieldData.ContainsKey(newData.Name))
                {
                    MessageBox.Show("Entry already exists. Use Edit to modify the entry");
                    return;
                }

                lbDetails.Items.Add(newData);
                itemManager.ShieldData.Add(newData.Name, newData);
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormShieldDetails frmShieldDetails = new FormShieldDetails())
            {
                frmShieldDetails.ShowDialog();

                if (frmShieldDetails.Shield != null)
                {
                    AddShield(frmShieldDetails.Shield);
                }
            }
        }

        #endregion

        #region Method region

        public void FillListBox()
        {
            lbDetails.Items.Clear();

            foreach (string s in FormDetails.ItemManager.ShieldData.Keys)
            {
                lbDetails.Items.Add(FormDetails.ItemManager.ShieldData[s]);
            }
        }

        private void AddShield(ShieldData shieldData)
        {
            if (FormDetails.ItemManager.ShieldData.ContainsKey(shieldData.Name))
            {
                DialogResult result = MessageBox.Show(
                    shieldData.Name + " already exists. Do you want to Overwrite it?",
                    "Entry exists",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    return;
                }

                itemManager.ShieldData[shieldData.Name] = shieldData;
                FillListBox();
                return;
            }

            itemManager.ShieldData.Add(shieldData.Name, shieldData);
            lbDetails.Items.Add(shieldData);
        }

        #endregion
    }
}
