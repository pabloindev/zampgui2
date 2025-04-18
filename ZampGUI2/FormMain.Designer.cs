﻿using System.Windows.Forms;

namespace ZampGUI2
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            menuStrip1 = new MenuStrip();
            serverToolStripMenuItem1 = new ToolStripMenuItem();
            editHTTPPortToolStripMenuItem = new ToolStripMenuItem();
            changeDefaultEditorToolStripMenuItem = new ToolStripMenuItem();
            changeVersionApachePHPToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesAtStartupToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            apacheHttpdToolStripMenuItem = new ToolStripMenuItem();
            apacheHttpdvhostsconfToolStripMenuItem = new ToolStripMenuItem();
            phpiniToolStripMenuItem = new ToolStripMenuItem();
            mariadbiniToolStripMenuItem = new ToolStripMenuItem();
            hostsFileToolStripMenuItem = new ToolStripMenuItem();
            zampGUIIniToolStripMenuItem = new ToolStripMenuItem();
            linksToolStripMenuItem = new ToolStripMenuItem();
            infophpToolStripMenuItem = new ToolStripMenuItem();
            phpmyadminToolStripMenuItem = new ToolStripMenuItem();
            consoleToolStripMenuItem_Console = new ToolStripMenuItem();
            openConsoleToolStripMenuItem = new ToolStripMenuItem();
            wordpressSaveInstanceToolStripMenuItem = new ToolStripMenuItem();
            wordpressRestoreInstanceToolStripMenuItem = new ToolStripMenuItem();
            wordpressDeleteInstanceToolStripMenuItem = new ToolStripMenuItem();
            wordpressScriptToolStripMenuItem = new ToolStripMenuItem();
            backupDatabasesToolStripMenuItem = new ToolStripMenuItem();
            runSqlScriptsToolStripMenuItem = new ToolStripMenuItem();
            foldersToolStripMenuItem = new ToolStripMenuItem();
            apacheFolderToolStripMenuItem = new ToolStripMenuItem();
            pHPFolderToolStripMenuItem = new ToolStripMenuItem();
            mariaDBFOLDERToolStripMenuItem = new ToolStripMenuItem();
            apacheHtdocsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            docsToolStripMenuItem = new ToolStripMenuItem();
            checkUpdateToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            label_uuid = new Label();
            label_wpcli = new Label();
            label_sass = new Label();
            label_node = new Label();
            label_git = new Label();
            label_composer = new Label();
            label_php = new Label();
            label_mariadb = new Label();
            label_apache = new Label();
            label_path = new Label();
            btnStartStopApache = new Button();
            btnStartStopMariadb = new Button();
            pictureBox_apache = new PictureBox();
            pictureBox_mariadb = new PictureBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            textBoxOuput = new DarkTextBox();
            timer_refreshFormProcess = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_apache).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_mariadb).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(37, 37, 38);
            menuStrip1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.ForeColor = Color.White;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { serverToolStripMenuItem1, editToolStripMenuItem, linksToolStripMenuItem, consoleToolStripMenuItem_Console, foldersToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(915, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // serverToolStripMenuItem1
            // 
            serverToolStripMenuItem1.CheckOnClick = true;
            serverToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { editHTTPPortToolStripMenuItem, changeDefaultEditorToolStripMenuItem, changeVersionApachePHPToolStripMenuItem, checkForUpdatesAtStartupToolStripMenuItem, exitToolStripMenuItem });
            serverToolStripMenuItem1.Name = "serverToolStripMenuItem1";
            serverToolStripMenuItem1.Size = new Size(55, 20);
            serverToolStripMenuItem1.Text = "Server";
            // 
            // editHTTPPortToolStripMenuItem
            // 
            editHTTPPortToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            editHTTPPortToolStripMenuItem.ForeColor = Color.White;
            editHTTPPortToolStripMenuItem.Name = "editHTTPPortToolStripMenuItem";
            editHTTPPortToolStripMenuItem.Size = new Size(243, 22);
            editHTTPPortToolStripMenuItem.Text = "Edit HTTP Port";
            editHTTPPortToolStripMenuItem.Click += editHTTPPortToolStripMenuItem_Click;
            // 
            // changeDefaultEditorToolStripMenuItem
            // 
            changeDefaultEditorToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            changeDefaultEditorToolStripMenuItem.ForeColor = Color.White;
            changeDefaultEditorToolStripMenuItem.Name = "changeDefaultEditorToolStripMenuItem";
            changeDefaultEditorToolStripMenuItem.Size = new Size(243, 22);
            changeDefaultEditorToolStripMenuItem.Text = "Change default editor";
            changeDefaultEditorToolStripMenuItem.Click += changeDefaultEditorToolStripMenuItem_Click;
            // 
            // changeVersionApachePHPToolStripMenuItem
            // 
            changeVersionApachePHPToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            changeVersionApachePHPToolStripMenuItem.ForeColor = Color.White;
            changeVersionApachePHPToolStripMenuItem.Name = "changeVersionApachePHPToolStripMenuItem";
            changeVersionApachePHPToolStripMenuItem.Size = new Size(243, 22);
            changeVersionApachePHPToolStripMenuItem.Text = "Change Version Apache/PHP";
            changeVersionApachePHPToolStripMenuItem.Click += changeVersionApachePHPToolStripMenuItem_Click;
            // 
            // checkForUpdatesAtStartupToolStripMenuItem
            // 
            checkForUpdatesAtStartupToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            checkForUpdatesAtStartupToolStripMenuItem.CheckOnClick = true;
            checkForUpdatesAtStartupToolStripMenuItem.ForeColor = Color.White;
            checkForUpdatesAtStartupToolStripMenuItem.Name = "checkForUpdatesAtStartupToolStripMenuItem";
            checkForUpdatesAtStartupToolStripMenuItem.Size = new Size(243, 22);
            checkForUpdatesAtStartupToolStripMenuItem.Text = "Check for updates at startup";
            checkForUpdatesAtStartupToolStripMenuItem.Click += checkForUpdatesAtStartupToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            exitToolStripMenuItem.ForeColor = Color.White;
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(243, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { apacheHttpdToolStripMenuItem, apacheHttpdvhostsconfToolStripMenuItem, phpiniToolStripMenuItem, mariadbiniToolStripMenuItem, hostsFileToolStripMenuItem, zampGUIIniToolStripMenuItem });
            editToolStripMenuItem.ForeColor = Color.White;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(42, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // apacheHttpdToolStripMenuItem
            // 
            apacheHttpdToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            apacheHttpdToolStripMenuItem.ForeColor = Color.White;
            apacheHttpdToolStripMenuItem.Name = "apacheHttpdToolStripMenuItem";
            apacheHttpdToolStripMenuItem.Size = new Size(218, 22);
            apacheHttpdToolStripMenuItem.Text = "apache httpd.conf";
            apacheHttpdToolStripMenuItem.Click += apacheHttpdToolStripMenuItem_Click;
            // 
            // apacheHttpdvhostsconfToolStripMenuItem
            // 
            apacheHttpdvhostsconfToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            apacheHttpdvhostsconfToolStripMenuItem.ForeColor = Color.White;
            apacheHttpdvhostsconfToolStripMenuItem.Name = "apacheHttpdvhostsconfToolStripMenuItem";
            apacheHttpdvhostsconfToolStripMenuItem.Size = new Size(218, 22);
            apacheHttpdvhostsconfToolStripMenuItem.Text = "apache httpd-vhosts.conf";
            apacheHttpdvhostsconfToolStripMenuItem.Click += apacheHttpdvhostsconfToolStripMenuItem_Click;
            // 
            // phpiniToolStripMenuItem
            // 
            phpiniToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            phpiniToolStripMenuItem.ForeColor = Color.White;
            phpiniToolStripMenuItem.Name = "phpiniToolStripMenuItem";
            phpiniToolStripMenuItem.Size = new Size(218, 22);
            phpiniToolStripMenuItem.Text = "php.ini";
            phpiniToolStripMenuItem.Click += phpiniToolStripMenuItem_Click;
            // 
            // mariadbiniToolStripMenuItem
            // 
            mariadbiniToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            mariadbiniToolStripMenuItem.ForeColor = Color.White;
            mariadbiniToolStripMenuItem.Name = "mariadbiniToolStripMenuItem";
            mariadbiniToolStripMenuItem.Size = new Size(218, 22);
            mariadbiniToolStripMenuItem.Text = "mariadb.ini";
            mariadbiniToolStripMenuItem.Click += mariadbiniToolStripMenuItem_Click;
            // 
            // hostsFileToolStripMenuItem
            // 
            hostsFileToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            hostsFileToolStripMenuItem.ForeColor = Color.White;
            hostsFileToolStripMenuItem.Name = "hostsFileToolStripMenuItem";
            hostsFileToolStripMenuItem.Size = new Size(218, 22);
            hostsFileToolStripMenuItem.Text = "hosts file";
            hostsFileToolStripMenuItem.Click += hostsFileToolStripMenuItem_Click;
            // 
            // zampGUIIniToolStripMenuItem
            // 
            zampGUIIniToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            zampGUIIniToolStripMenuItem.ForeColor = Color.White;
            zampGUIIniToolStripMenuItem.Name = "zampGUIIniToolStripMenuItem";
            zampGUIIniToolStripMenuItem.Size = new Size(218, 22);
            zampGUIIniToolStripMenuItem.Text = "ZampGUI ini";
            zampGUIIniToolStripMenuItem.Click += zampGUIIniToolStripMenuItem_Click;
            // 
            // linksToolStripMenuItem
            // 
            linksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { infophpToolStripMenuItem, phpmyadminToolStripMenuItem });
            linksToolStripMenuItem.ForeColor = Color.White;
            linksToolStripMenuItem.Name = "linksToolStripMenuItem";
            linksToolStripMenuItem.Size = new Size(50, 20);
            linksToolStripMenuItem.Text = "Links";
            // 
            // infophpToolStripMenuItem
            // 
            infophpToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            infophpToolStripMenuItem.ForeColor = Color.White;
            infophpToolStripMenuItem.Name = "infophpToolStripMenuItem";
            infophpToolStripMenuItem.Size = new Size(148, 22);
            infophpToolStripMenuItem.Text = "info.php";
            infophpToolStripMenuItem.Click += infophpToolStripMenuItem_Click;
            // 
            // phpmyadminToolStripMenuItem
            // 
            phpmyadminToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            phpmyadminToolStripMenuItem.ForeColor = Color.White;
            phpmyadminToolStripMenuItem.Name = "phpmyadminToolStripMenuItem";
            phpmyadminToolStripMenuItem.Size = new Size(148, 22);
            phpmyadminToolStripMenuItem.Text = "phpmyadmin";
            phpmyadminToolStripMenuItem.Click += phpmyadminToolStripMenuItem_Click;
            // 
            // consoleToolStripMenuItem_Console
            // 
            consoleToolStripMenuItem_Console.DropDownItems.AddRange(new ToolStripItem[] { openConsoleToolStripMenuItem, wordpressSaveInstanceToolStripMenuItem, wordpressRestoreInstanceToolStripMenuItem, wordpressDeleteInstanceToolStripMenuItem, wordpressScriptToolStripMenuItem, backupDatabasesToolStripMenuItem, runSqlScriptsToolStripMenuItem });
            consoleToolStripMenuItem_Console.ForeColor = Color.White;
            consoleToolStripMenuItem_Console.Name = "consoleToolStripMenuItem_Console";
            consoleToolStripMenuItem_Console.Size = new Size(66, 20);
            consoleToolStripMenuItem_Console.Text = "Console";
            // 
            // openConsoleToolStripMenuItem
            // 
            openConsoleToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            openConsoleToolStripMenuItem.ForeColor = Color.White;
            openConsoleToolStripMenuItem.Name = "openConsoleToolStripMenuItem";
            openConsoleToolStripMenuItem.Size = new Size(239, 22);
            openConsoleToolStripMenuItem.Text = "Open Console";
            openConsoleToolStripMenuItem.Click += openConsoleToolStripMenuItem_Click;
            // 
            // wordpressSaveInstanceToolStripMenuItem
            // 
            wordpressSaveInstanceToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            wordpressSaveInstanceToolStripMenuItem.ForeColor = Color.White;
            wordpressSaveInstanceToolStripMenuItem.Name = "wordpressSaveInstanceToolStripMenuItem";
            wordpressSaveInstanceToolStripMenuItem.Size = new Size(239, 22);
            wordpressSaveInstanceToolStripMenuItem.Text = "Wordpress Save Instance";
            wordpressSaveInstanceToolStripMenuItem.Click += wordpressSaveInstanceToolStripMenuItem_Click;
            // 
            // wordpressRestoreInstanceToolStripMenuItem
            // 
            wordpressRestoreInstanceToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            wordpressRestoreInstanceToolStripMenuItem.ForeColor = Color.White;
            wordpressRestoreInstanceToolStripMenuItem.Name = "wordpressRestoreInstanceToolStripMenuItem";
            wordpressRestoreInstanceToolStripMenuItem.Size = new Size(239, 22);
            wordpressRestoreInstanceToolStripMenuItem.Text = "Wordpress Restore Instance";
            wordpressRestoreInstanceToolStripMenuItem.Click += wordpressRestoreInstanceToolStripMenuItem_Click;
            // 
            // wordpressDeleteInstanceToolStripMenuItem
            // 
            wordpressDeleteInstanceToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            wordpressDeleteInstanceToolStripMenuItem.ForeColor = Color.White;
            wordpressDeleteInstanceToolStripMenuItem.Name = "wordpressDeleteInstanceToolStripMenuItem";
            wordpressDeleteInstanceToolStripMenuItem.Size = new Size(239, 22);
            wordpressDeleteInstanceToolStripMenuItem.Text = "Wordpress Delete Instance";
            wordpressDeleteInstanceToolStripMenuItem.Click += wordpressDeleteInstanceToolStripMenuItem_Click;
            // 
            // wordpressScriptToolStripMenuItem
            // 
            wordpressScriptToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            wordpressScriptToolStripMenuItem.ForeColor = Color.White;
            wordpressScriptToolStripMenuItem.Name = "wordpressScriptToolStripMenuItem";
            wordpressScriptToolStripMenuItem.Size = new Size(239, 22);
            wordpressScriptToolStripMenuItem.Text = "WordPress New Instance";
            wordpressScriptToolStripMenuItem.Click += wordpressNewInstanceToolStripMenuItem_Click;
            // 
            // backupDatabasesToolStripMenuItem
            // 
            backupDatabasesToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            backupDatabasesToolStripMenuItem.ForeColor = Color.White;
            backupDatabasesToolStripMenuItem.Name = "backupDatabasesToolStripMenuItem";
            backupDatabasesToolStripMenuItem.Size = new Size(239, 22);
            backupDatabasesToolStripMenuItem.Text = "Backup Databases";
            backupDatabasesToolStripMenuItem.Click += backupDatabasesToolStripMenuItem_Click;
            // 
            // runSqlScriptsToolStripMenuItem
            // 
            runSqlScriptsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            runSqlScriptsToolStripMenuItem.ForeColor = Color.White;
            runSqlScriptsToolStripMenuItem.Name = "runSqlScriptsToolStripMenuItem";
            runSqlScriptsToolStripMenuItem.Size = new Size(239, 22);
            runSqlScriptsToolStripMenuItem.Text = "Run sql Scripts";
            runSqlScriptsToolStripMenuItem.Click += runSqlScriptsToolStripMenuItem_Click;
            // 
            // foldersToolStripMenuItem
            // 
            foldersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { apacheFolderToolStripMenuItem, pHPFolderToolStripMenuItem, mariaDBFOLDERToolStripMenuItem, apacheHtdocsToolStripMenuItem });
            foldersToolStripMenuItem.Name = "foldersToolStripMenuItem";
            foldersToolStripMenuItem.Size = new Size(62, 20);
            foldersToolStripMenuItem.Text = "Folders";
            // 
            // apacheFolderToolStripMenuItem
            // 
            apacheFolderToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            apacheFolderToolStripMenuItem.ForeColor = Color.White;
            apacheFolderToolStripMenuItem.Name = "apacheFolderToolStripMenuItem";
            apacheFolderToolStripMenuItem.Size = new Size(164, 22);
            apacheFolderToolStripMenuItem.Text = "Apache Folder";
            apacheFolderToolStripMenuItem.Click += apacheFolderToolStripMenuItem_Click;
            // 
            // pHPFolderToolStripMenuItem
            // 
            pHPFolderToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            pHPFolderToolStripMenuItem.ForeColor = Color.White;
            pHPFolderToolStripMenuItem.Name = "pHPFolderToolStripMenuItem";
            pHPFolderToolStripMenuItem.Size = new Size(164, 22);
            pHPFolderToolStripMenuItem.Text = "PHP Folder";
            pHPFolderToolStripMenuItem.Click += pHPFolderToolStripMenuItem_Click;
            // 
            // mariaDBFOLDERToolStripMenuItem
            // 
            mariaDBFOLDERToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            mariaDBFOLDERToolStripMenuItem.ForeColor = Color.White;
            mariaDBFOLDERToolStripMenuItem.Name = "mariaDBFOLDERToolStripMenuItem";
            mariaDBFOLDERToolStripMenuItem.Size = new Size(164, 22);
            mariaDBFOLDERToolStripMenuItem.Text = "MariaDB Folder";
            mariaDBFOLDERToolStripMenuItem.Click += mariaDBFOLDERToolStripMenuItem_Click;
            // 
            // apacheHtdocsToolStripMenuItem
            // 
            apacheHtdocsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            apacheHtdocsToolStripMenuItem.ForeColor = Color.White;
            apacheHtdocsToolStripMenuItem.Name = "apacheHtdocsToolStripMenuItem";
            apacheHtdocsToolStripMenuItem.Size = new Size(164, 22);
            apacheHtdocsToolStripMenuItem.Text = "Apache htdocs";
            apacheHtdocsToolStripMenuItem.Click += apacheHtdocsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { docsToolStripMenuItem, checkUpdateToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.ForeColor = Color.White;
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(45, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // docsToolStripMenuItem
            // 
            docsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            docsToolStripMenuItem.ForeColor = Color.White;
            docsToolStripMenuItem.Name = "docsToolStripMenuItem";
            docsToolStripMenuItem.Size = new Size(154, 22);
            docsToolStripMenuItem.Text = "Docs";
            docsToolStripMenuItem.Click += docsToolStripMenuItem_Click;
            // 
            // checkUpdateToolStripMenuItem
            // 
            checkUpdateToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            checkUpdateToolStripMenuItem.ForeColor = Color.White;
            checkUpdateToolStripMenuItem.Name = "checkUpdateToolStripMenuItem";
            checkUpdateToolStripMenuItem.Size = new Size(154, 22);
            checkUpdateToolStripMenuItem.Text = "Check update";
            checkUpdateToolStripMenuItem.Click += checkUpdateToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            aboutToolStripMenuItem.ForeColor = Color.White;
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(154, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(label_uuid);
            panel1.Controls.Add(label_wpcli);
            panel1.Controls.Add(label_sass);
            panel1.Controls.Add(label_node);
            panel1.Controls.Add(label_git);
            panel1.Controls.Add(label_composer);
            panel1.Controls.Add(label_php);
            panel1.Controls.Add(label_mariadb);
            panel1.Controls.Add(label_apache);
            panel1.Controls.Add(label_path);
            panel1.Location = new Point(11, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(717, 132);
            panel1.TabIndex = 3;
            // 
            // label_uuid
            // 
            label_uuid.AutoSize = true;
            label_uuid.Location = new Point(195, 42);
            label_uuid.Name = "label_uuid";
            label_uuid.Size = new Size(39, 17);
            label_uuid.TabIndex = 9;
            label_uuid.Text = "uuid:";
            // 
            // label_wpcli
            // 
            label_wpcli.AutoSize = true;
            label_wpcli.Location = new Point(370, 71);
            label_wpcli.Name = "label_wpcli";
            label_wpcli.Size = new Size(53, 17);
            label_wpcli.TabIndex = 8;
            label_wpcli.Text = "wp_cli:";
            // 
            // label_sass
            // 
            label_sass.AutoSize = true;
            label_sass.Location = new Point(370, 99);
            label_sass.Name = "label_sass";
            label_sass.Size = new Size(44, 17);
            label_sass.TabIndex = 7;
            label_sass.Text = "sass:";
            // 
            // label_node
            // 
            label_node.AutoSize = true;
            label_node.Location = new Point(195, 99);
            label_node.Name = "label_node";
            label_node.Size = new Size(44, 17);
            label_node.TabIndex = 6;
            label_node.Text = "node:";
            // 
            // label_git
            // 
            label_git.AutoSize = true;
            label_git.Location = new Point(195, 71);
            label_git.Name = "label_git";
            label_git.Size = new Size(27, 17);
            label_git.TabIndex = 5;
            label_git.Text = "git:";
            // 
            // label_composer
            // 
            label_composer.AutoSize = true;
            label_composer.Location = new Point(533, 71);
            label_composer.Name = "label_composer";
            label_composer.Size = new Size(78, 17);
            label_composer.TabIndex = 4;
            label_composer.Text = "composer:";
            // 
            // label_php
            // 
            label_php.AutoSize = true;
            label_php.Location = new Point(13, 99);
            label_php.Name = "label_php";
            label_php.Size = new Size(40, 17);
            label_php.TabIndex = 3;
            label_php.Text = "php: ";
            // 
            // label_mariadb
            // 
            label_mariadb.AutoSize = true;
            label_mariadb.Location = new Point(13, 71);
            label_mariadb.Name = "label_mariadb";
            label_mariadb.Size = new Size(69, 17);
            label_mariadb.TabIndex = 2;
            label_mariadb.Text = "mariadb: ";
            // 
            // label_apache
            // 
            label_apache.AutoSize = true;
            label_apache.Location = new Point(13, 42);
            label_apache.Name = "label_apache";
            label_apache.Size = new Size(64, 17);
            label_apache.TabIndex = 1;
            label_apache.Text = "apache: ";
            // 
            // label_path
            // 
            label_path.AutoSize = true;
            label_path.Location = new Point(13, 14);
            label_path.Name = "label_path";
            label_path.Size = new Size(44, 17);
            label_path.TabIndex = 0;
            label_path.Text = "path: ";
            // 
            // btnStartStopApache
            // 
            btnStartStopApache.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStartStopApache.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnStartStopApache.FlatStyle = FlatStyle.Flat;
            btnStartStopApache.Location = new Point(787, 69);
            btnStartStopApache.Name = "btnStartStopApache";
            btnStartStopApache.Size = new Size(117, 34);
            btnStartStopApache.TabIndex = 4;
            btnStartStopApache.Text = "Start Apache";
            btnStartStopApache.UseVisualStyleBackColor = true;
            btnStartStopApache.Click += btnStartStopApache_Click;
            // 
            // btnStartStopMariadb
            // 
            btnStartStopMariadb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStartStopMariadb.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnStartStopMariadb.FlatStyle = FlatStyle.Flat;
            btnStartStopMariadb.Location = new Point(787, 126);
            btnStartStopMariadb.Name = "btnStartStopMariadb";
            btnStartStopMariadb.Size = new Size(117, 34);
            btnStartStopMariadb.TabIndex = 5;
            btnStartStopMariadb.Text = "Start MariaDB";
            btnStartStopMariadb.UseVisualStyleBackColor = true;
            btnStartStopMariadb.Click += btnStartStopMariadb_Click;
            // 
            // pictureBox_apache
            // 
            pictureBox_apache.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_apache.Image = Properties.Resources.cancel;
            pictureBox_apache.Location = new Point(746, 69);
            pictureBox_apache.Name = "pictureBox_apache";
            pictureBox_apache.Size = new Size(32, 34);
            pictureBox_apache.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_apache.TabIndex = 7;
            pictureBox_apache.TabStop = false;
            // 
            // pictureBox_mariadb
            // 
            pictureBox_mariadb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_mariadb.Image = Properties.Resources.cancel;
            pictureBox_mariadb.Location = new Point(746, 126);
            pictureBox_mariadb.Name = "pictureBox_mariadb";
            pictureBox_mariadb.Size = new Size(32, 34);
            pictureBox_mariadb.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_mariadb.TabIndex = 8;
            pictureBox_mariadb.TabStop = false;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = Color.FromArgb(64, 64, 64);
            statusStrip1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            statusStrip1.Location = new Point(0, 489);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(915, 21);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(131, 16);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // textBoxOuput
            // 
            textBoxOuput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxOuput.BackColor = Color.FromArgb(30, 30, 30);
            textBoxOuput.BorderStyle = BorderStyle.FixedSingle;
            textBoxOuput.ForeColor = Color.White;
            textBoxOuput.Location = new Point(12, 184);
            textBoxOuput.Multiline = true;
            textBoxOuput.Name = "textBoxOuput";
            textBoxOuput.ReadOnly = true;
            textBoxOuput.ScrollBars = ScrollBars.Vertical;
            textBoxOuput.Size = new Size(891, 293);
            textBoxOuput.TabIndex = 11;
            // 
            // timer_refreshFormProcess
            // 
            timer_refreshFormProcess.Enabled = true;
            timer_refreshFormProcess.Interval = 15000;
            timer_refreshFormProcess.Tick += timer_refreshFormProcess_Tick;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(915, 510);
            Controls.Add(textBoxOuput);
            Controls.Add(statusStrip1);
            Controls.Add(pictureBox_mariadb);
            Controls.Add(pictureBox_apache);
            Controls.Add(btnStartStopMariadb);
            Controls.Add(btnStartStopApache);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(931, 549);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_apache).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_mariadb).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem serverToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem apacheHttpdToolStripMenuItem;
        private ToolStripMenuItem apacheHttpdvhostsconfToolStripMenuItem;
        private ToolStripMenuItem phpiniToolStripMenuItem;
        private ToolStripMenuItem mariadbiniToolStripMenuItem;
        private ToolStripMenuItem linksToolStripMenuItem;
        private ToolStripMenuItem infophpToolStripMenuItem;
        private ToolStripMenuItem phpmyadminToolStripMenuItem;
        private ToolStripMenuItem consoleToolStripMenuItem_Console;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem docsToolStripMenuItem;
        private ToolStripMenuItem checkUpdateToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Panel panel1;
        private Label label_path;
        private Button btnStartStopApache;
        private Button btnStartStopMariadb;
        private Label label_sass;
        private Label label_node;
        private Label label_git;
        private Label label_composer;
        private Label label_php;
        private Label label_mariadb;
        private Label label_apache;
        private Label label_uuid;
        private Label label_wpcli;
        private PictureBox pictureBox_apache;
        private PictureBox pictureBox_mariadb;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem openConsoleToolStripMenuItem;
        private ToolStripMenuItem editHTTPPortToolStripMenuItem;
        private ToolStripMenuItem changeDefaultEditorToolStripMenuItem;
        private ToolStripMenuItem hostsFileToolStripMenuItem;
        private ToolStripMenuItem wordpressScriptToolStripMenuItem;
        private ToolStripMenuItem foldersToolStripMenuItem;
        private ToolStripMenuItem apacheFolderToolStripMenuItem;
        private ToolStripMenuItem pHPFolderToolStripMenuItem;
        private ToolStripMenuItem mariaDBFOLDERToolStripMenuItem;
        private ToolStripMenuItem apacheHtdocsToolStripMenuItem;
        private ToolStripMenuItem changeVersionApachePHPToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesAtStartupToolStripMenuItem;
        public DarkTextBox textBoxOuput;
        private System.Windows.Forms.Timer timer_refreshFormProcess;
        private ToolStripMenuItem zampGUIIniToolStripMenuItem;
        private ToolStripMenuItem backupDatabasesToolStripMenuItem;
        private ToolStripMenuItem runSqlScriptsToolStripMenuItem;
        private ToolStripMenuItem wordpressSaveInstanceToolStripMenuItem;
        private ToolStripMenuItem wordpressRestoreInstanceToolStripMenuItem;
        private ToolStripMenuItem wordpressDeleteInstanceToolStripMenuItem;
    }
}
