﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using RimWorld;
using Verse;
using UnityEngine;

namespace CommunityCoreLibrary.Detour
{

    internal static class _MainMenuDrawer
    {

        private const float         GameRectWidth = 200f;
        private const float         NewsRectWidth = 350f;
        private const int           ButCount = 3;
        private const float         TitleShift = 50f;

        private static MethodInfo   _CloseMainTab;

        private static FieldInfo    _anyWorldFiles;
        private static FieldInfo    _anyMapFiles;

        private static FieldInfo    _PaneSize;
        private static FieldInfo    _TitleSize;

        private static FieldInfo    _IconBlog;
        private static FieldInfo    _IconForums;
        private static FieldInfo    _IconTwitter;
        private static FieldInfo    _IconBook;

        private static FieldInfo    _TexTitle;

        private static FieldInfo    _TexLudeonLogo;
        private static FieldInfo    _LudeonLogoSize;

        static _MainMenuDrawer()
        {
            _CloseMainTab   = typeof( MainMenuDrawer ).GetMethod( "CloseMainTab", BindingFlags.Static | BindingFlags.NonPublic );

            _anyWorldFiles  = typeof( MainMenuDrawer ).GetField( "anyWorldFiles", BindingFlags.Static | BindingFlags.NonPublic );
            _anyMapFiles    = typeof( MainMenuDrawer ).GetField( "anyMapFiles", BindingFlags.Static | BindingFlags.NonPublic );

            _PaneSize       = typeof( MainMenuDrawer ).GetField( "PaneSize", BindingFlags.Static | BindingFlags.NonPublic );
            _TitleSize      = typeof( MainMenuDrawer ).GetField( "TitleSize", BindingFlags.Static | BindingFlags.NonPublic );

            var PaneSize    = (Vector2) _PaneSize.GetValue( null );
            PaneSize.y      += 104f;

            _PaneSize.SetValue( null, PaneSize );

            _IconBlog       = typeof( MainMenuDrawer ).GetField( "IconBlog", BindingFlags.Static | BindingFlags.NonPublic );
            _IconForums     = typeof( MainMenuDrawer ).GetField( "IconForums", BindingFlags.Static | BindingFlags.NonPublic );
            _IconTwitter    = typeof( MainMenuDrawer ).GetField( "IconTwitter", BindingFlags.Static | BindingFlags.NonPublic );
            _IconBook       = typeof( MainMenuDrawer ).GetField( "IconBook", BindingFlags.Static | BindingFlags.NonPublic );

            _TexTitle       = typeof( MainMenuDrawer ).GetField( "TexTitle", BindingFlags.Static | BindingFlags.NonPublic );

            _TexLudeonLogo  = typeof( MainMenuDrawer ).GetField( "TexLudeonLogo", BindingFlags.Static | BindingFlags.NonPublic );
            _LudeonLogoSize = typeof( MainMenuDrawer ).GetField( "LudeonLogoSize", BindingFlags.Static | BindingFlags.NonPublic );
        }

        internal static void _MainMenuOnGUI()
        {
            var anyWorldFiles   = (bool) _anyWorldFiles.GetValue( null );
            var anyMapFiles     = (bool) _anyMapFiles.GetValue( null );
            var PaneSize        = (Vector2) _PaneSize.GetValue( null );
            var LudeonLogoSize  = (Vector2) _LudeonLogoSize.GetValue( null );

            VersionControl.DrawInfoInCorner();

            var menuOptionsRect = new Rect(
                ( (float) Screen.width  - PaneSize.x ) / 2f,
                ( (float) Screen.height - PaneSize.y ) / 2f,
                PaneSize.x,
                PaneSize.y );

            // CCL Change to accomodate additional buttons: TitleShift -> TitleShift *2f
            menuOptionsRect.y += TitleShift * 2f;

            menuOptionsRect.x = ( (float) Screen.width - menuOptionsRect.width - 30.0f );

            var titleBaseVec = (Vector2) _TitleSize.GetValue( null );
            if( titleBaseVec.x > (float) Screen.width )
            {
                titleBaseVec *= (float) Screen.width / titleBaseVec.x;
            }
            var titleFinalVec = titleBaseVec * 0.7f;

            var titleRect = new Rect(
                ( (float) Screen.width - titleFinalVec.x ) / 2f,
                (float) ( menuOptionsRect.y - titleFinalVec.y - 10.0 ),
                titleFinalVec.x,
                titleFinalVec.y );
            titleRect.x = ( (float) Screen.width - titleFinalVec.x - TitleShift );
            GUI.DrawTexture(
                titleRect,
                (Texture) _TexTitle.GetValue( null ),
                ScaleMode.StretchToFill,
                true );

            var mainCreditRect = titleRect;
            mainCreditRect.y += titleRect.height;
            mainCreditRect.xMax -= 55f;
            mainCreditRect.height = 30f;
            mainCreditRect.y += 3f;
            var mainCreditText = "MainPageCredit".Translate();
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperRight;
            if( Screen.width < 990 )
            {
                var mainCreditBackRect = mainCreditRect;
                mainCreditBackRect.xMin = mainCreditBackRect.xMax - Text.CalcSize( mainCreditText ).x;
                mainCreditBackRect.xMin -= 4f;
                mainCreditBackRect.xMax += 4f;
                GUI.color = new Color( 0.2f, 0.2f, 0.2f, 0.5f );
                GUI.DrawTexture(
                    mainCreditBackRect,
                    (Texture) BaseContent.WhiteTex );
                GUI.color = Color.white;
            }
            Widgets.Label( mainCreditRect, mainCreditText );
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;

            GUI.color = new Color( 1f, 1f, 1f, 0.5f );
            GUI.DrawTexture(
                new Rect(
                    (float) ( Screen.width - 8 ) - LudeonLogoSize.x,
                    8f,
                    LudeonLogoSize.x,
                    LudeonLogoSize.y ),
                (Texture) _TexLudeonLogo.GetValue( null ),
                ScaleMode.StretchToFill,
                true );
            GUI.color = Color.white;

            var menuOptionsContractedRect = menuOptionsRect.ContractedBy( 17f );
            GUI.BeginGroup( menuOptionsContractedRect );
            MainMenuDrawer.DoMainMenuButtons(
                menuOptionsContractedRect,
                anyWorldFiles,
                anyMapFiles );
            GUI.EndGroup();
        }

        internal static void _DoMainMenuButtons( Rect rect, bool anyWorldFiles, bool anyMapFiles, Action backToGameButtonAction = null )
        {
            Rect mainOptionRect = new Rect( 0.0f, 0.0f, 200f, rect.height );
            Rect linkOptionAreaRect = new Rect( mainOptionRect.xMax + 17f, 0.0f, -1f, rect.height );
            linkOptionAreaRect.xMax = rect.width;
            Text.Font = GameFont.Small;

            List<ListableOption> mainOptions = new List<ListableOption>();

            if( Game.Mode == GameMode.Entry )
            {
                mainOptions.Add( _CreateWorldOption() );

                if( anyWorldFiles )
                {
                    mainOptions.Add( _NewColonyOption() );
                }
            }

            if( Game.Mode == GameMode.MapPlaying )
            {
                if( backToGameButtonAction != null )
                {
                    mainOptions.Add( new ListableOption(
                        "BackToGame".Translate(),
                        backToGameButtonAction ) );
                }

                mainOptions.Add( _SaveGameOption() );
            }

            if( anyMapFiles )
            {
                mainOptions.Add( _LoadGameOption() );
            }

            mainOptions.Add( _MainOptionsOption() );

            if( Game.Mode == GameMode.Entry )
            {
                mainOptions.Add( _ModsOption() );
            }

            // CCL Menu Insertion
            if( Window_ModConfigurationMenu.AnyMenus )
            {
                mainOptions.Add( _ModOptionsOption() );
            }

            if( Game.Mode == GameMode.Entry )
            {
                mainOptions.Add( _HelpMenuOption() );
                mainOptions.Add( _CreditsOption() );
            }

            if( Game.Mode == GameMode.MapPlaying )
            {
                mainOptions.Add( _QuitToMainOption() );
            }

            mainOptions.Add( _QuitToOSOption() );

            double mainOptionsHeight = (double) OptionListingUtility.DrawOptionListing( GenUI.ContractedBy( mainOptionRect, 17f ), mainOptions );

            Text.Font = GameFont.Small;

            List<ListableOption> linkOptions = new List<ListableOption>()
            {
                _FictionPrimerOption(),
                _BlogOption(),
                _ForumsOption(),
                _WikiOption(),
                _TwitterOption(),
                _DesignBookOption(),
                _TranslateOption()
            };
            Rect linkOptionRect = linkOptionAreaRect.ContractedBy( 17f );
            float linkOptionHeight = OptionListingUtility.DrawOptionListing( linkOptionRect, linkOptions );

            if( Game.Mode == GameMode.Entry )
            {
                GUI.BeginGroup( linkOptionRect );
                if(
                    Widgets.ImageButton(
                        new Rect(
                            0.0f,
                            linkOptionHeight + 10f,
                            64f,
                            32f ),
                        LanguageDatabase.activeLanguage.icon )
                )
                {
                    var languageOptions = new List<FloatMenuOption>();
                    foreach( LoadedLanguage loadedLanguage in LanguageDatabase.AllLoadedLanguages )
                    {
                        var switcher = new SwitchLang( loadedLanguage );
                        languageOptions.Add(
                            new FloatMenuOption(
                                switcher.localLang.FriendlyNameNative,
                                switcher.SwitchTo,
                                MenuOptionPriority.Medium )
                        );
                    }
                    Find.WindowStack.Add( (Window) new FloatMenu( languageOptions, false ) );
                }
                GUI.EndGroup();
            }
        }

        internal class SwitchLang
        {

            public LoadedLanguage localLang;

            public SwitchLang( LoadedLanguage lang )
            {
                localLang = lang;
            }

            public void SwitchTo()
            {
                LanguageDatabase.SelectLanguage( localLang );
                Prefs.Save();
            }

        }

        #region Option Buttons

        internal static ListableOption  _CreateWorldOption()
        {
            return new ListableOption(
                "CreateWorld".Translate(),
                () =>
            {
                MapInitData.Reset();
                Find.WindowStack.Add( (Window) new Page_CreateWorldParams() );
            } );
        }

        internal static ListableOption  _NewColonyOption()
        {
            return new ListableOption(
                "NewColony".Translate(),
                () =>
            {
                MapInitData.Reset();
                Find.WindowStack.Add( (Window) new Page_SelectStoryteller() );
            } );
        }

        internal static ListableOption  _SaveGameOption()
        {
            return new ListableOption(
                "Save".Translate(),
                () =>
            {
                _CloseMainTab.Invoke( null, null );
                Find.WindowStack.Add( (Window) new Dialog_MapList_Save() );
            } );
        }

        internal static ListableOption  _LoadGameOption()
        {
            return new ListableOption(
                "Load".Translate(),
                () =>
            {
                _CloseMainTab.Invoke( null, null );
                Find.WindowStack.Add( (Window) new Dialog_MapList_Load() );
            } );
        }

        internal static ListableOption  _MainOptionsOption()
        {
            return new ListableOption(
                "Options".Translate(),
                () =>
            {
                _CloseMainTab.Invoke( null, null );
                Find.WindowStack.Add( (Window) new Dialog_Options() );
            } );
        }

        internal static ListableOption  _ModsOption()
        {
            return new ListableOption(
                "Mods".Translate(),
                () =>
            {
                Find.WindowStack.Add( (Window) new Page_ModsConfig() );
            } );
        }

        internal static ListableOption  _ModOptionsOption()
        {
            return new ListableOption(
                "ModConfigurationOptions".Translate(),
                () =>
            {
                _CloseMainTab.Invoke( null, null );
                Find.WindowStack.Add( (Window) new Window_ModConfigurationMenu() );
            } );
        }

        internal static ListableOption  _HelpMenuOption()
        {
            return new ListableOption(
                "HelpOTabTitle".Translate(),
                () =>
            {
                Find.WindowStack.Add( (Window) new MainTabWindow_ModHelp() );
            } );
        }

        internal static ListableOption  _CreditsOption()
        {
            return new ListableOption(
                "Credits".Translate(),
                () =>
            {
                Find.WindowStack.Add( (Window) new Page_Credits() );
            } );
        }

        internal static ListableOption  _QuitToMainOption()
        {
            return new ListableOption(
                "QuitToMainMenu".Translate(),
                _ConfirmQuitToMain );
        }

        internal static ListableOption  _QuitToOSOption()
        {
            ListableOption option = null;
            if( Game.Mode == GameMode.MapPlaying )
            {
                option = new ListableOption(
                    "QuitToOS".Translate(),
                    _ConfirmQuitToOS );
            }
            else
            {
                option = new ListableOption(
                    "QuitToOS".Translate(),
                    Root.Shutdown );
            }
            return option;
        }

        /*
        internal static ListableOption  _Option()
        {
            return new ListableOption(
                "".Translate(),
                () =>
            {
            } );
        }
        */

        #endregion

        #region Link Buttons

        internal static ListableOption  _FictionPrimerOption()
        {
            return new ListableOption_WebLink(
                "FictionPrimer".Translate(),
                "https://docs.google.com/document/d/1pIZyKif0bFbBWten4drrm7kfSSfvBoJPgG9-ywfN8j8/pub",
                (Texture2D) _IconBlog.GetValue( null )
            );
        }

        internal static ListableOption  _BlogOption()
        {
            return new ListableOption_WebLink(
                "LudeonBlog".Translate(),
                "http://ludeon.com/blog",
                (Texture2D) _IconBlog.GetValue( null )
            );
        }

        internal static ListableOption  _ForumsOption()
        {
            return new ListableOption_WebLink(
                "Forums".Translate(),
                "http://ludeon.com/forums",
                (Texture2D) _IconForums.GetValue( null )
            );
        }

        internal static ListableOption  _WikiOption()
        {
            return new ListableOption_WebLink(
                "OfficialWiki".Translate(),
                "http://rimworldwiki.com",
                (Texture2D) _IconBlog.GetValue( null )
            );
        }

        internal static ListableOption  _TwitterOption()
        {
            return new ListableOption_WebLink(
                "TynansTwitter".Translate(),
                "https://twitter.com/TynanSylvester",
                (Texture2D) _IconTwitter.GetValue( null )
            );
        }

        internal static ListableOption  _DesignBookOption()
        {
            return new ListableOption_WebLink(
                "TynansDesignBook".Translate(),
                "http://tynansylvester.com/book",
                (Texture2D) _IconBook.GetValue( null )
            );
        }

        internal static ListableOption  _TranslateOption()
        {
            return new ListableOption_WebLink(
                "HelpTranslate".Translate(),
                "http://ludeon.com/forums/index.php?topic=2933.0",
                (Texture2D) _IconForums.GetValue( null )
            );
        }

        /*
        internal static ListableOption  _Option()
        {
            return new ListableOption_WebLink(
                "".Translate(),
                "",
                (Texture2D) _Icon.GetValue( null )
            );
        }
        */

        #endregion

        #region Quiter!

        internal static void            _ConfirmQuitToMain()
        {
            Find.WindowStack.Add( (Window) new Dialog_Confirm(
                "ConfirmQuit".Translate(),
                () =>
            {
                Application.LoadLevel( "Entry" );
            },
                false
            ) );
        }

        internal static void            _ConfirmQuitToOS()
        {
            Find.WindowStack.Add( (Window) new Dialog_Confirm(
                "ConfirmQuit".Translate(),
                Root.Shutdown,
                false
            ) );
        }

        #endregion

    }

}
