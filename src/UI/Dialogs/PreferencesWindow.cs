using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Gwen;
using Gwen.Controls;
using OpenTK.Graphics;
using linerider.Tools;
using linerider.Utils;
using linerider.Game;

namespace linerider.UI
{
    public class PreferencesWindow : DialogBase
    {
        private CollapsibleList _prefcontainer;
        private ControlBase _focus;
        private int _tabscount = 0;
        public PreferencesWindow(GameCanvas parent, Editor editor) : base(parent, editor)
        {
            Title = "Preferences";
            SetSize(450, 425);
            MinimumSize = Size;
            ControlBase bottom = new ControlBase(this)
            {
                Dock = Dock.Bottom,
                AutoSizeToContents = true,
            };
            Button defaults = new Button(bottom)
            {
                Dock = Dock.Right,
                Margin = new Margin(0, 2, 0, 0),
                Text = "Restore Defaults"
            };
            defaults.Clicked += (o, e) => RestoreDefaults();
            _prefcontainer = new CollapsibleList(this)
            {
                Dock = Dock.Left,
                AutoSizeToContents = false,
                Width = 100,
                Margin = new Margin(0, 0, 5, 0)
            };
            MakeModal(true);
            Setup();
        }
        private void RestoreDefaults()
        {
            var mbox = MessageBox.Show(
                _canvas,
                "Are you sure? This cannot be undone.", "Restore Defaults",
                MessageBox.ButtonType.OkCancel,
                true);
            mbox.RenameButtons("Restore");
            mbox.Dismissed += (o, e) =>
            {
                if (e == DialogResult.OK)
                {
                    Settings.RestoreDefaultSettings();
                    Settings.Save();
                    _editor.InitCamera();
                    Close();// this is lazy, but i don't want to update the ui
                }
            };
        }

        private void PopulateConfigToggle(ControlBase parent)
        {
            var configt = GwenHelper.CreateHeaderPanel(parent, "Configuration");
            GwenHelper.AddCheckbox(configt, "Contact Points", Settings.ConfigTContactPoints, (o, e) =>
            {
                Settings.ConfigTContactPoints = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            GwenHelper.AddCheckbox(configt, "Gravity Wells", Settings.ConfigTGravityWells, (o, e) =>
            {
                Settings.ConfigTGravityWells = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            GwenHelper.AddCheckbox(configt, "Hit Test", Settings.ConfigTHitTest, (o, e) =>
            {
                Settings.ConfigTHitTest = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            GwenHelper.AddCheckbox(configt, "Momentum Vectors", Settings.ConfigTMomentumVectors, (o, e) =>
            {
                Settings.ConfigTMomentumVectors = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            GwenHelper.AddCheckbox(configt, "Onion Skinning", Settings.ConfigTOnionSkinning, (o, e) =>
            {
                Settings.ConfigTOnionSkinning = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
        }

        // Individual line options

        private void PopulateAccelLine(ControlBase parent)
        {
            var accel = GwenHelper.CreateHeaderPanel(parent, "Line Options");
            GwenHelper.AddCheckbox(accel, "Line Customization", Settings.AccelerationColorChange, (o, e) =>
            {
                Settings.AccelerationColorChange = ((Checkbox)o).IsChecked;
                Settings.Save();
            });


            var coloraccel = GwenHelper.CreateHeaderPanel(parent, "RGB");
            var redS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.AccelerationColorRed,
            };
            redS.ValueChanged += (o, e) =>
            {
                Settings.AccelerationColorRed = (int)redS.Value;
                Constants.AccelerationRed = (int)redS.Value;
                Settings.Save();
                Constants.RedLineColored = Color.FromArgb(Constants.AccelerationRed, Constants.AccelerationGreen, Constants.AccelerationBlue);
            };
            GwenHelper.CreateLabeledControl(coloraccel, "Red", redS);
            var greenS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.AccelerationColorGreen,
            };
            greenS.ValueChanged += (o, e) =>
            {
                Settings.AccelerationColorGreen = (int)greenS.Value;
                Constants.AccelerationGreen = (int)greenS.Value;
                Settings.Save();
                Constants.RedLineColored = Color.FromArgb(Constants.AccelerationRed, Constants.AccelerationGreen, Constants.AccelerationBlue);
            };
            GwenHelper.CreateLabeledControl(coloraccel, "Green", greenS);
            var blueS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.AccelerationColorBlue,
            };
            blueS.ValueChanged += (o, e) =>
            {
                Settings.AccelerationColorBlue = (int)blueS.Value;
                Constants.AccelerationBlue = (int)blueS.Value;
                Settings.Save();
                Constants.RedLineColored = Color.FromArgb(Constants.AccelerationRed, Constants.AccelerationGreen, Constants.AccelerationBlue);
            };
            GwenHelper.CreateLabeledControl(coloraccel, "Blue", blueS);
        }

        private void PopulateNormalLine(ControlBase parent)
        {
            var normal = GwenHelper.CreateHeaderPanel(parent, "Line Options");
            GwenHelper.AddCheckbox(normal, "Line Customization", Settings.NormalColorChange, (o, e) =>
            {
                Settings.NormalColorChange = ((Checkbox)o).IsChecked;
                Settings.Save();
            });


            var colornormal = GwenHelper.CreateHeaderPanel(parent, "RGB");
            var redS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.NormalColorRed,
            };
            redS.ValueChanged += (o, e) =>
            {
                Settings.NormalColorRed = (int)redS.Value;
                Constants.NormalRed = (int)redS.Value;
                Settings.Save();
                Constants.BlueLineColored = Color.FromArgb(Constants.NormalRed, Constants.NormalGreen, Constants.NormalBlue);
            };
            GwenHelper.CreateLabeledControl(colornormal, "Red", redS);
            var greenS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.NormalColorGreen,
            };
            greenS.ValueChanged += (o, e) =>
            {
                Settings.NormalColorGreen = (int)greenS.Value;
                Constants.NormalGreen = (int)greenS.Value;
                Settings.Save();
                Constants.BlueLineColored = Color.FromArgb(Constants.NormalRed, Constants.NormalGreen, Constants.NormalBlue);
            };
            GwenHelper.CreateLabeledControl(colornormal, "Green", greenS);
            var blueS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.NormalColorBlue,
            };
            blueS.ValueChanged += (o, e) =>
            {
                Settings.NormalColorBlue = (int)blueS.Value;
                Constants.NormalBlue = (int)blueS.Value;
                Settings.Save();
                Constants.BlueLineColored = Color.FromArgb(Constants.NormalRed, Constants.NormalGreen, Constants.NormalBlue);
            };
            GwenHelper.CreateLabeledControl(colornormal, "Blue", blueS);
        }

        private void PopulateSceneryLine(ControlBase parent)
        {
            var scenery = GwenHelper.CreateHeaderPanel(parent, "Line Options");
            GwenHelper.AddCheckbox(scenery, "Line Customization", Settings.SceneryColorChange, (o, e) =>
            {
                Settings.SceneryColorChange = ((Checkbox)o).IsChecked;
                Settings.Save();
            });


            var colorscenery = GwenHelper.CreateHeaderPanel(parent, "RGB");
            var redS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.SceneryColorRed,
            };
            redS.ValueChanged += (o, e) =>
            {
                Settings.SceneryColorRed = (int)redS.Value;
                Constants.SceneryRed = (int)redS.Value;
                Settings.Save();
                Constants.SceneryLineColored = Color.FromArgb(Constants.SceneryRed, Constants.SceneryGreen, Constants.SceneryBlue);
            };
            GwenHelper.CreateLabeledControl(colorscenery, "Red", redS);
            var greenS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.SceneryColorGreen,
            };
            greenS.ValueChanged += (o, e) =>
            {
                Settings.SceneryColorGreen = (int)greenS.Value;
                Constants.SceneryGreen = (int)greenS.Value;
                Settings.Save();
                Constants.SceneryLineColored = Color.FromArgb(Constants.SceneryRed, Constants.SceneryGreen, Constants.SceneryBlue);
            };
            GwenHelper.CreateLabeledControl(colorscenery, "Green", greenS);
            var blueS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.SceneryColorBlue,
            };
            blueS.ValueChanged += (o, e) =>
            {
                Settings.SceneryColorBlue = (int)blueS.Value;
                Constants.SceneryBlue = (int)blueS.Value;
                Settings.Save();
                Constants.SceneryLineColored = Color.FromArgb(Constants.SceneryRed, Constants.SceneryGreen, Constants.SceneryBlue);
            };
            GwenHelper.CreateLabeledControl(colorscenery, "Blue", blueS);
        }

        // End of Individual line options

        private void PopulateLines2(ControlBase parent)
        {
            var lineoptions2 = GwenHelper.CreateHeaderPanel(parent, "XY Lock options");
            var xylock = new Spinner(null)
            {
                Max = 180,
                Min = 1,
                Value = Settings.XY,
            };
            xylock.ValueChanged += (o, e) =>
            {
                Settings.XY = (float)xylock.Value;
                Settings.Save();
            };
            GwenHelper.CreateLabeledControl(lineoptions2, "XY Degrees", xylock);
        }

        private void PopulateLines(ControlBase parent)
        {
            var lineoptions = GwenHelper.CreateHeaderPanel(parent, "Line options");
            GwenHelper.AddCheckbox(lineoptions, "Line Customization", Settings.MainLine, (o, e) =>
            {
                Settings.MainLine = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var coloroptions = GwenHelper.CreateHeaderPanel(parent, "Color options");
            var redS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.LineColorRed,
            };
            redS.ValueChanged += (o, e) =>
            {
                Settings.LineColorRed = (int)redS.Value;
                Constants.LineRed = (int)redS.Value;
                Settings.Save();
                Constants.ColorDefaultLine = Color.FromArgb(Constants.LineRed, Constants.LineGreen, Constants.LineBlue);
            };
            GwenHelper.CreateLabeledControl(coloroptions, "Red", redS);
            var greenS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.LineColorGreen,
            };
            greenS.ValueChanged += (o, e) =>
            {
                Settings.LineColorGreen = (int)greenS.Value;
                Constants.LineGreen = (int)greenS.Value;
                Settings.Save();
                Constants.ColorDefaultLine = Color.FromArgb(Constants.LineRed, Constants.LineGreen, Constants.LineBlue);
            };
            GwenHelper.CreateLabeledControl(coloroptions, "Green", greenS);
            var blueS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.LineColorBlue,
            };
            blueS.ValueChanged += (o, e) =>
            {
                Settings.LineColorBlue = (int)blueS.Value;
                Constants.LineBlue = (int)blueS.Value;
                Settings.Save();
                Constants.ColorDefaultLine = Color.FromArgb(Constants.LineRed, Constants.LineGreen, Constants.LineBlue);
            };
            GwenHelper.CreateLabeledControl(coloroptions, "Blue", blueS);
        }
        private void PopulateAudio(ControlBase parent)
        {
            var opts = GwenHelper.CreateHeaderPanel(parent, "Sync options");
            var syncenabled = GwenHelper.AddCheckbox(opts, "Mute", Settings.MuteAudio, (o, e) =>
               {
                   Settings.MuteAudio = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            HorizontalSlider vol = new HorizontalSlider(null)
            {
                Min = 0,
                Max = 100,
                Value = Settings.Volume,
                Width = 80,
            };
            vol.ValueChanged += (o, e) =>
              {
                  Settings.Volume = (float)vol.Value;
                  Settings.Save();
              };
            GwenHelper.CreateLabeledControl(opts, "Volume", vol);
            vol.Width = 200;
        }
        private void PopulateKeybinds(ControlBase parent)
        {
            var hk = new HotkeyWidget(parent);
        }
        private void PopulateModes(ControlBase parent)
        {
            var background = GwenHelper.CreateHeaderPanel(parent, "Background Color");
            GwenHelper.AddCheckbox(background, "Night Mode", Settings.NightMode, (o, e) =>
               {
                   Settings.NightMode = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var whitebg = GwenHelper.AddCheckbox(background, "Pure White Background", Settings.WhiteBG, (o, e) =>
               {
                   Settings.WhiteBG = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var coloredbg = GwenHelper.AddCheckbox(background, "Colored Background", Settings.ColoredBG, (o, e) =>
            {
                Settings.ColoredBG = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var panelgeneral = GwenHelper.CreateHeaderPanel(parent, "General");
            var superzoom = GwenHelper.AddCheckbox(panelgeneral, "Superzoom", Settings.SuperZoom, (o, e) =>
               {
                   Settings.SuperZoom = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            ComboBox scroll = GwenHelper.CreateLabeledCombobox(panelgeneral, "Scroll Sensitivity:");
            scroll.Margin = new Margin(0, 0, 0, 0);
            scroll.Dock = Dock.Bottom;
            scroll.AddItem("0.25x").Name = "0.25";
            scroll.AddItem("0.5x").Name = "0.5";
            scroll.AddItem("0.75x").Name = "0.75";
            scroll.AddItem("1x").Name = "1";
            scroll.AddItem("2x").Name = "2";
            scroll.AddItem("3x").Name = "3";
            scroll.SelectByName("1");//default if user setting fails.
            scroll.SelectByName(Settings.ScrollSensitivity.ToString(Program.Culture));
            scroll.ItemSelected += (o, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Settings.ScrollSensitivity = float.Parse(e.SelectedItem.Name, Program.Culture);
                    Settings.Save();
                }
            };
            superzoom.Tooltip = "Allows the user to zoom in\nnearly 10x more than usual.";

            var colbg = GwenHelper.CreateHeaderPanel(parent, "Colored Background RGB");
            var redS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.RedColored,
            };
            redS.ValueChanged += (o, e) =>
            {
                Settings.RedColored = (int)redS.Value;
                Constants.Red = (int)redS.Value;
                Settings.Save();
                Constants.ColorColored = new Color4((float)(Constants.Red / 255), (float)(Constants.Green / 255), (float)(Constants.Blue / 255), 255);
            };
            GwenHelper.CreateLabeledControl(colbg, "Red", redS);
            var greenS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.GreenColored,
            };
            greenS.ValueChanged += (o, e) =>
            {
                Settings.GreenColored = (int)greenS.Value;
                Constants.Green = (int)greenS.Value;
                Settings.Save();
                Constants.ColorColored = new Color4((float)(Constants.Red / 255), (float)(Constants.Green / 255), (float)(Constants.Blue / 255), 255);
            };
            GwenHelper.CreateLabeledControl(colbg, "Green", greenS);
            var blueS = new Spinner(null)
            {
                Min = 0,
                Max = 255,
                Value = Settings.BlueColored,
            };
            blueS.ValueChanged += (o, e) =>
            {
                Settings.BlueColored = (int)blueS.Value;
                Constants.Blue = (int)blueS.Value;
                Settings.Save();
                Constants.ColorColored = new Color4((float)(Constants.Red / 255), (float)(Constants.Green / 255), (float)(Constants.Blue / 255), 255);
            };
            GwenHelper.CreateLabeledControl(colbg, "Blue", blueS);
        }
        private void PopulateCamera(ControlBase parent)
        {
            var camtype = GwenHelper.CreateHeaderPanel(parent, "Camera Type");
            var camtracking = GwenHelper.CreateHeaderPanel(parent, "Camera Tracking");
            var camprops = GwenHelper.CreateHeaderPanel(parent, "Camera Properties");
            RadioButtonGroup rbcamera = new RadioButtonGroup(camtype)
            {
                Dock = Dock.Top,
                ShouldDrawBackground = false,
            };
            var soft = rbcamera.AddOption("Soft Camera");
            var predictive = rbcamera.AddOption("Predictive Camera");
            var legacy = rbcamera.AddOption("Legacy Camera");

            RadioButtonGroup xycamera = new RadioButtonGroup(camtracking)
            {
                Dock = Dock.Top,
                ShouldDrawBackground = false,
            };
            var horizontal = xycamera.AddOption("Horizontal Tracking");
            var vertical = xycamera.AddOption("Vertical Tracking");
            var horizontalvertical = xycamera.AddOption("Normal Tracking");

            var round = GwenHelper.AddCheckbox(camprops, "Round Legacy Camera", Settings.RoundLegacyCamera, (o, e) =>
            {
                Settings.RoundLegacyCamera = ((Checkbox)o).IsChecked;
                Settings.Save();
                _editor.InitCamera();
            });
            var offsledsled = GwenHelper.AddCheckbox(camprops, "Offsled Sled Camera", Settings.OffsledSledCam, (o, e) =>
            {
            Settings.OffsledSledCam = ((Checkbox)o).IsChecked;
            Settings.Save();
            _editor.InitCamera();
            });
            var offsledvar = GwenHelper.AddCheckbox(camprops, "Variable Contact Point Camera", Settings.OffsledVar, (o, e) =>
            {
                Settings.OffsledVar = ((Checkbox)o).IsChecked;
                Settings.Save();
                _editor.InitCamera();
            });
            var fixedcam = GwenHelper.AddCheckbox(camprops, "Fixed Camera", Settings.FixedCam, (o, e) =>
            {
                Settings.FixedCam = ((Checkbox)o).IsChecked;
                Settings.Save();
                _editor.InitCamera();
            });
            var variables = GwenHelper.CreateHeaderPanel(parent, "Contact Point Settings");
            Spinner pointvar = new Spinner(variables)
            {
                Dock = Dock.Bottom,
                Max = 29,
                Min = 0,
                Value = Settings.PointVar,
            };
            pointvar.ValueChanged += (o, e) =>
            {
                Settings.PointVar = (int)pointvar.Value;
                Settings.Save();
            };
            var fixedpos = GwenHelper.CreateHeaderPanel(parent, "Fixed Camera X and Y Position");
            var xfixed = new Spinner(null)
            {
                Dock = Dock.Bottom,
                Max = 2147483648,
                Min = -2147483648,
                Value = Settings.XFixed 
            };
            xfixed.ValueChanged += (o, e) =>
            {
                Settings.XFixed = (int)xfixed.Value;
                Settings.Save();
                _editor.InitCamera();
            };
            var yfixed = new Spinner(null)
            {
                Dock = Dock.Bottom,
                Max = 2147483648,
                Min = -2147483648,
                Value = Settings.YFixed
            };
            yfixed.ValueChanged += (o, e) =>
            {
                Settings.YFixed = (int)yfixed.Value;
                Settings.Save();
                _editor.InitCamera();
            };
            GwenHelper.CreateLabeledControl(fixedpos, "X Location", xfixed);
            GwenHelper.CreateLabeledControl(fixedpos, "Y Location", yfixed);
            if (Settings.SmoothCamera)
            {
                if (Settings.PredictiveCamera)
                    predictive.Select();
                else
                    soft.Select();
            }
            else
            {
                legacy.Select();
            }

            if (Settings.HorizontalTracking)
            {
                horizontal.Select();
            }
            else if (Settings.VerticalTracking)
            {
                vertical.Select();
            }
            else horizontalvertical.Select();

            horizontal.Checked += (o, e) =>
            {
                Settings.HorizontalTracking = true;
                Settings.VerticalTracking = false;
                Settings.Save();
                _editor.InitCamera();
            };
            vertical.Checked += (o, e) =>
            {
                Settings.HorizontalTracking = false;
                Settings.VerticalTracking = true;
                Settings.Save();
                _editor.InitCamera();
            };
            horizontalvertical.Checked += (o, e) =>
            {
                Settings.HorizontalTracking = false;
                Settings.VerticalTracking = false;
                Settings.Save();
                _editor.InitCamera();

            };

            soft.Checked += (o, e) =>
            {
                Settings.SmoothCamera = true;
                Settings.PredictiveCamera = false;
                Settings.Save();
                round.IsDisabled = Settings.SmoothCamera;
                _editor.InitCamera();
            };
            predictive.Checked += (o, e) =>
            {
                Settings.SmoothCamera = true;
                Settings.PredictiveCamera = true;
                Settings.Save();
                round.IsDisabled = Settings.SmoothCamera;
                _editor.InitCamera();
            };
            legacy.Checked += (o, e) =>
            {
                Settings.SmoothCamera = false;
                Settings.PredictiveCamera = false;
                Settings.Save();
                round.IsDisabled = Settings.SmoothCamera;
                _editor.InitCamera();

            };
            predictive.Tooltip = "This is the camera that was added in 1.03\nIt moves relative to the future of the track";
        }
        private void PopulateEditor(ControlBase parent)
        {
            Panel advancedtools = GwenHelper.CreateHeaderPanel(parent, "Advanced Visualization");

            var contact = GwenHelper.AddCheckbox(advancedtools, "Contact Points", Settings.Editor.DrawContactPoints, (o, e) =>
            {
                Settings.Editor.DrawContactPoints = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var momentum = GwenHelper.AddCheckbox(advancedtools, "Momentum Vectors", Settings.Editor.MomentumVectors, (o, e) =>
            {
                Settings.Editor.MomentumVectors = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var hitbox = GwenHelper.AddCheckbox(advancedtools, "Line Hitbox", Settings.Editor.RenderGravityWells, (o, e) =>
            {
                Settings.Editor.RenderGravityWells = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var hittest = GwenHelper.AddCheckbox(advancedtools, "Hit Test", Settings.Editor.HitTest, (o, e) =>
             {
                 Settings.Editor.HitTest = ((Checkbox)o).IsChecked;
                 Settings.Save();
             });
            var onion = GwenHelper.AddCheckbox(advancedtools, "Onion Skinning", Settings.OnionSkinning, (o, e) =>
            {
                Settings.OnionSkinning = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            Panel pblifelock = GwenHelper.CreateHeaderPanel(parent, "Lifelock Conditions");
            GwenHelper.AddCheckbox(pblifelock, "Next frame constraints", Settings.Editor.LifeLockNoOrange, (o, e) =>
            {
                Settings.Editor.LifeLockNoOrange = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            GwenHelper.AddCheckbox(pblifelock, "No Fakie Death", Settings.Editor.LifeLockNoFakie, (o, e) =>
            {
                Settings.Editor.LifeLockNoFakie = ((Checkbox)o).IsChecked;
                Settings.Save();
            });

            var overlay = GwenHelper.CreateHeaderPanel(parent, "Frame Overlay");
            PopulateOverlay(overlay);
            
            onion.Tooltip = "Visualize the rider before/after\nthe current frame.";
            momentum.Tooltip = "Visualize the direction of\nmomentum for each contact point";
            contact.Tooltip = "Visualize the parts of the rider\nthat interact with lines.";
            hitbox.Tooltip = "Visualizes the hitbox of lines\nUsed for advanced editing";
            hittest.Tooltip = "Lines that have been hit by\nthe rider will glow.";
        }
        private void PopulatePlayback(ControlBase parent)
        {
            var playbackzoom = GwenHelper.CreateHeaderPanel(parent, "Playback Zoom");
            RadioButtonGroup pbzoom = new RadioButtonGroup(playbackzoom)
            {
                Dock = Dock.Left,
                ShouldDrawBackground = false,
            };
            pbzoom.AddOption("Default Zoom");
            pbzoom.AddOption("Current Zoom");
            pbzoom.AddOption("Specific Zoom");
            Spinner playbackspinner = new Spinner(pbzoom)
            {
                Dock = Dock.Bottom,
                Max = 24,
                Min = 1,
            };
            pbzoom.SelectionChanged += (o, e) =>
            {
                Settings.PlaybackZoomType = ((RadioButtonGroup)o).SelectedIndex;
                Settings.Save();
                playbackspinner.IsHidden = (((RadioButtonGroup)o).SelectedLabel != "Specific Zoom");
            };
            playbackspinner.ValueChanged += (o, e) =>
            {
                Settings.PlaybackZoomValue = (float)((Spinner)o).Value;
                Settings.Save();
            };
            pbzoom.SetSelection(Settings.PlaybackZoomType);
            playbackspinner.Value = Settings.PlaybackZoomValue;

            var playbackmode = GwenHelper.CreateHeaderPanel(parent, "Playback Color");
            GwenHelper.AddCheckbox(playbackmode, "Color Playback", Settings.ColorPlayback, (o, e) =>
               {
                   Settings.ColorPlayback = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var preview = GwenHelper.AddCheckbox(playbackmode, "Preview Mode", Settings.PreviewMode, (o, e) =>
               {
                   Settings.PreviewMode = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var recording = GwenHelper.AddCheckbox(playbackmode, "Recording Mode", Settings.Local.RecordingMode, (o, e) =>
               {
                   Settings.Local.RecordingMode = ((Checkbox)o).IsChecked;
               });
            var framerate = GwenHelper.CreateHeaderPanel(parent, "Frame Control");
            var smooth = GwenHelper.AddCheckbox(framerate, "Smooth Playback", Settings.SmoothPlayback, (o, e) =>
               {
                   Settings.SmoothPlayback = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            ComboBox pbrate = GwenHelper.CreateLabeledCombobox(framerate, "Playback Rate:");
            for (var i = 0; i < Constants.MotionArray.Length; i++)
            {
                var f = (Constants.MotionArray[i] / (float)Constants.PhysicsRate);
                pbrate.AddItem(f + "x", f.ToString(CultureInfo.InvariantCulture), f);
            }
            pbrate.SelectByName(Settings.DefaultPlayback.ToString(CultureInfo.InvariantCulture));
            pbrate.ItemSelected += (o, e) =>
            {
                Settings.DefaultPlayback = (float)e.SelectedItem.UserData;
                Settings.Save();
            };
            var cbslowmo = GwenHelper.CreateLabeledCombobox(framerate, "Slowmo FPS:");
            var fpsarray = new[] { 1, 2, 5, 10, 20 };
            for (var i = 0; i < fpsarray.Length; i++)
            {
                cbslowmo.AddItem(fpsarray[i].ToString(), fpsarray[i].ToString(CultureInfo.InvariantCulture),
                    fpsarray[i]);
            }
            cbslowmo.SelectByName(Settings.SlowmoSpeed.ToString(CultureInfo.InvariantCulture));
            cbslowmo.ItemSelected += (o, e) =>
            {
                Settings.SlowmoSpeed = (int)e.SelectedItem.UserData;
                Settings.Save();
            };
            smooth.Tooltip = "Interpolates frames from the base\nphysics rate of 40 frames/second\nup to 60 frames/second";
        }
        private void PopulateOverlay(ControlBase parent)
        {
            var offset = new Spinner(null)
            {
                Min = -999,
                Max = 999,
                Value = Settings.Local.TrackOverlayOffset,
            };
            offset.ValueChanged += (o,e)=>
            {
                Settings.Local.TrackOverlayOffset = (int)offset.Value;
            };
            var fixedspinner = new Spinner(null)
            {
                Min = 0,
                Max = _editor.FrameCount,
                Value = Settings.Local.TrackOverlayFixedFrame,
            };
            fixedspinner.ValueChanged += (o, e) =>
            {
                Settings.Local.TrackOverlayFixedFrame = (int)fixedspinner.Value;
            };
            void updatedisabled()
            {
                offset.IsDisabled = Settings.Local.TrackOverlayFixed;
                fixedspinner.IsDisabled = !Settings.Local.TrackOverlayFixed;
            }
            var enabled = GwenHelper.AddCheckbox(parent, "Enabled", Settings.Local.TrackOverlay, (o, e) =>
            {
                Settings.Local.TrackOverlay = ((Checkbox)o).IsChecked;
                updatedisabled();
            });
            GwenHelper.AddCheckbox(parent, "Fixed Frame", Settings.Local.TrackOverlayFixed, (o, e) =>
            {
                Settings.Local.TrackOverlayFixed = ((Checkbox)o).IsChecked;
                updatedisabled();
            });
            GwenHelper.CreateLabeledControl(parent, "Frame Offset", offset);
            GwenHelper.CreateLabeledControl(parent, "Frame ID", fixedspinner);
            updatedisabled();
            enabled.Tooltip = "Display an onion skin of the track\nat a specified offset for animation";
        }
        private void PopulateTools(ControlBase parent)
        {
            var select = GwenHelper.CreateHeaderPanel(parent, "Select Tool -- Line Info");
            var length = GwenHelper.AddCheckbox(select, "Show Length", Settings.Editor.ShowLineLength, (o, e) =>
               {
                   Settings.Editor.ShowLineLength = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var angle = GwenHelper.AddCheckbox(select, "Show Angle", Settings.Editor.ShowLineAngle, (o, e) =>
               {
                   Settings.Editor.ShowLineAngle = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var showid = GwenHelper.AddCheckbox(select, "Show ID", Settings.Editor.ShowLineID, (o, e) =>
               {
                   Settings.Editor.ShowLineID = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            Panel panelSnap = GwenHelper.CreateHeaderPanel(parent, "Snapping");
            var linesnap = GwenHelper.AddCheckbox(panelSnap, "Snap New Lines", Settings.Editor.SnapNewLines, (o, e) =>
            {
                Settings.Editor.SnapNewLines = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var movelinesnap = GwenHelper.AddCheckbox(panelSnap, "Snap Line Movement", Settings.Editor.SnapMoveLine, (o, e) =>
            {
                Settings.Editor.SnapMoveLine = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var forcesnap = GwenHelper.AddCheckbox(panelSnap, "Force X/Y snap", Settings.Editor.ForceXySnap, (o, e) =>
            {
                Settings.Editor.ForceXySnap = ((Checkbox)o).IsChecked;
                Settings.Save();
            });
            var onsk = GwenHelper.CreateHeaderPanel(parent, "Onion Skinning Options");
            var osb = new Spinner(null)
            {
                Min = 0,
                Max = 100,
                Value = Settings.OnionSkinningBack,
            };
            osb.ValueChanged += (o, e) =>
            {
                Settings.OnionSkinningBack = (int)osb.Value;
                Settings.Save();
            };
            GwenHelper.CreateLabeledControl(onsk, "Onionskinning Back", osb);

            var osf = new Spinner(null)
            {
                Min = 0,
                Max = 100,
                Value = Settings.OnionSkinningFront,
            };
            osf.ValueChanged += (o, e) =>
            {
                Settings.OnionSkinningFront = (int)osf.Value;
                Settings.Save();
            };
            GwenHelper.CreateLabeledControl(onsk, "Onionskinning Front", osf);

            forcesnap.Tooltip = "Forces all lines drawn to\nsnap to a 45 degree angle";
            movelinesnap.Tooltip = "Snap to lines when using the\nselect tool to move a single line";
        }
        private void PopulateOther(ControlBase parent)
        {
            var updates = GwenHelper.CreateHeaderPanel(parent, "Updates");

            var showid = GwenHelper.AddCheckbox(updates, "Check For Updates", Settings.CheckForUpdates, (o, e) =>
               {
                   Settings.CheckForUpdates = ((Checkbox)o).IsChecked;
                   Settings.Save();
               });
            var accel = GwenHelper.CreateHeaderPanel(parent, ("Time Elapsed: " + Settings.MinutesElapsed + " minutes"));

        }
        private void Setup()
        {
            var cat = _prefcontainer.Add("Settings");
            var page = AddPage(cat, "Editor");
            PopulateEditor(page);
            page = AddPage(cat, "Toggle Config");
            PopulateConfigToggle(page);
            page = AddPage(cat, "Playback");
            PopulatePlayback(page);
            page = AddPage(cat, "Tools");
            PopulateTools(page);
            page = AddPage(cat, "Environment");
            PopulateModes(page);
            page = AddPage(cat, "Line Settings");
            PopulateLines(page);
            page = AddPage(cat, "Line Settings 2");
            PopulateLines2(page);
            page = AddPage(cat, "Normal Lines");
            PopulateNormalLine(page);
            page = AddPage(cat, "Accel Lines");
            PopulateAccelLine(page);
            page = AddPage(cat, "Scenery Lines");
            PopulateSceneryLine(page);
            page = AddPage(cat, "Camera");
            PopulateCamera(page);
            cat = _prefcontainer.Add("Application");
            page = AddPage(cat, "Audio");
            PopulateAudio(page);
            page = AddPage(cat, "Keybindings");
            PopulateKeybinds(page);
            page = AddPage(cat, "Other");
            PopulateOther(page);
            if (Settings.SettingsPane >= _tabscount && _focus == null)
            {
                Settings.SettingsPane = 0;
                _focus = page;
                page.Show();
            }

        }
        private void CategorySelected(object sender, ItemSelectedEventArgs e)
        {
            if (_focus != e.SelectedItem.UserData)
            {
                if (_focus != null)
                {
                    _focus.Hide();
                }
                _focus = (ControlBase)e.SelectedItem.UserData;
                _focus.Show();
                Settings.SettingsPane = (int)_focus.UserData;
                Settings.Save();
            }
        }
        private ControlBase AddPage(CollapsibleCategory category, string name)
        {
            var btn = category.Add(name);
            Panel panel = new Panel(this);
            panel.Dock = Dock.Fill;
            panel.Padding = Padding.Five;
            panel.Hide();
            panel.UserData = _tabscount;
            btn.UserData = panel;
            category.Selected += CategorySelected;
            if (_tabscount == Settings.SettingsPane)
                btn.Press();
            _tabscount += 1;
            return panel;
        }
    }
}
