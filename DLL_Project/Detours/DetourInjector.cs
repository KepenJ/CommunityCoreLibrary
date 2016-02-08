﻿using System;
using System.Reflection;
using System.Security.Permissions;

using RimWorld;
using Verse;

namespace CommunityCoreLibrary
{

    public class DetourInjector : SpecialInjector
    {

        public override void                Inject()
        {

            Assembly Assembly_CSharp = Assembly.Load( "Assembly-CSharp.dll" );
#if DEBUG
            if( Assembly_CSharp == null )
            {
                CCL_Log.Error( "Unable to load 'Assembly-CSharp'" );
                return;
            }
#endif

            // Detour Verse.GenSpawn.CanPlaceBlueprintOver
            MethodInfo Verse_GenSpawn_CanPlaceBlueprintOver = typeof( GenSpawn ).GetMethod( "CanPlaceBlueprintOver", BindingFlags.Static | BindingFlags.Public );
            MethodInfo CCL_GenSpawn_CanPlaceBlueprintOver = typeof( Detour._GenSpawn ).GetMethod( "_CanPlaceBlueprintOver", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_GenSpawn_CanPlaceBlueprintOver, CCL_GenSpawn_CanPlaceBlueprintOver );

            // Detour Verse.RegionMaker.CheckRegionableAndProcessNeighbor
            Detour._RegionMaker._TryMakePortalSpan = typeof( RegionMaker ).GetMethod( "TryMakePortalSpan", BindingFlags.Static | BindingFlags.NonPublic );
#if DEBUG
            if( Detour._RegionMaker._TryMakePortalSpan == null )
            {
                CCL_Log.Error( "Unable to find 'Verse.RegionMaker.TryMakePortalScan'" );
                return;
            }
#endif
            MethodInfo Verse_RegionMaker_CheckRegionableAndProcessNeighbor = typeof( RegionMaker ).GetMethod( "CheckRegionableAndProcessNeighbor", BindingFlags.Static | BindingFlags.NonPublic, null, new [] { typeof( IntVec3 ), typeof( Rot4 ) }, null );
            MethodInfo CCL_RegionMaker_CheckRegionableAndProcessNeighbor = typeof( Detour._RegionMaker ).GetMethod( "_CheckRegionableAndProcessNeighbor", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_RegionMaker_CheckRegionableAndProcessNeighbor, CCL_RegionMaker_CheckRegionableAndProcessNeighbor );

            // Detour RimWorld.TargetingParameters.CanTarget
            MethodInfo RimWorld_TargetingParameters_CanTarget = typeof( TargetingParameters ).GetMethod( "CanTarget", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_TargetingParameters_CanTarget = typeof( Detour._TargetingParameters ).GetMethod( "_CanTarget", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_TargetingParameters_CanTarget, CCL_TargetingParameters_CanTarget );

            // Detour RimWorld.GenPlant.CanEverPlantAt
            MethodInfo RimWorld_GenPlant_CanEverPlantAt = typeof( GenPlant ).GetMethod( "CanEverPlantAt", BindingFlags.Static | BindingFlags.Public );
            MethodInfo CCL_GenPlant_CanEverPlantAt = typeof( Detour._GenPlant ).GetMethod( "_CanEverPlantAt", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_GenPlant_CanEverPlantAt, CCL_GenPlant_CanEverPlantAt );

            // Detour RimWorld.StatWorker.ShouldShowFor
            MethodInfo RimWorld_StatWorker_ShouldShowFor = typeof( StatWorker ).GetMethod( "ShouldShowFor", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_StatWorker_ShouldShowFor = typeof( Detour._StatWorker ).GetMethod( "_ShouldShowFor", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_StatWorker_ShouldShowFor, CCL_StatWorker_ShouldShowFor );

            // Detour Verse.SectionLayer_IndoorMask.Regenerate
            Type Verse_Sectionlayer_IndoorMask = Assembly_CSharp.GetType( "Verse.SectionLayer_IndoorMask" );
#if DEBUG
            if( Verse_Sectionlayer_IndoorMask == null )
            {
                CCL_Log.Error( "Unable to find 'Verse.Sectionlayer_IndoorMask'" );
                return;
            }
#endif
            Detour._SectionLayer_IndoorMask._HideRainPrimary = Verse_Sectionlayer_IndoorMask.GetMethod( "HideRainPrimary", BindingFlags.Instance | BindingFlags.NonPublic );
#if DEBUG
            if( Detour._SectionLayer_IndoorMask._HideRainPrimary == null )
            {
                CCL_Log.Error( "Unable to find 'Verse.Sectionlayer_IndoorMask.HideRainPrimary'" );
                return;
            }
#endif
            MethodInfo Verse_SectionLayer_IndoorMask_Regenerate = Verse_Sectionlayer_IndoorMask.GetMethod( "Regenerate", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_SectionLayer_IndoorMask_Regenerate = typeof( Detour._SectionLayer_IndoorMask ).GetMethod( "_Regenerate", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_SectionLayer_IndoorMask_Regenerate, CCL_SectionLayer_IndoorMask_Regenerate );

            // Detour RimWorld.Building_Door.AlignQualityAgainst
            MethodInfo RimWorld_Building_Door_AlignQualityAgainst = typeof( Building_Door ).GetMethod( "AlignQualityAgainst", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo CCL_Building_Door_AlignQualityAgainst = typeof( Detour._Building_Door ).GetMethod( "_AlignQualityAgainst", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_Building_Door_AlignQualityAgainst, CCL_Building_Door_AlignQualityAgainst );

            // Detour Verse.GhostDrawer.GhostGraphicFor
            MethodInfo Verse_GhostDrawer_GhostGraphicFor = typeof( GhostDrawer ).GetMethod( "GhostGraphicFor", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo CCL_GhostDrawer_GhostGraphicFor = typeof( Detour._GhostDrawer ).GetMethod( "_GhostGraphicFor", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_GhostDrawer_GhostGraphicFor, CCL_GhostDrawer_GhostGraphicFor );

            // Detour Verse.GenPlace.PlaceSpotQualityAt
            MethodInfo Verse_GenPlace_PlaceSpotQualityAt = typeof( GenPlace ).GetMethod( "PlaceSpotQualityAt", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo CCL_GenPlace_PlaceSpotQualityAt = typeof( Detour._GenPlace ).GetMethod( "_PlaceSpotQualityAt", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_GenPlace_PlaceSpotQualityAt, CCL_GenPlace_PlaceSpotQualityAt );

            // Detour RimWorld.JobGiver_Binge.DrinkAlcoholJob
            MethodInfo RimWorld_JobGiver_Binge_DrinkAlcoholJob = typeof( JobGiver_Binge ).GetMethod( "DrinkAlcoholJob", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo CCL_JobGiver_Binge_DrinkAlcoholJob = typeof( Detour._JobGiver_Binge ).GetMethod( "_DrinkAlcoholJob", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_JobGiver_Binge_DrinkAlcoholJob, CCL_JobGiver_Binge_DrinkAlcoholJob );

            // Detour Verse.BrokenStateWorker_Binging.StateCanOccur
            MethodInfo Verse_BrokenStateWorker_Binging_StateCanOccur = typeof( BrokenStateWorker_Binging ).GetMethod( "StateCanOccur", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_BrokenStateWorker_Binging_StateCanOccur = typeof( Detour._BrokenStateWorker_Binging ).GetMethod( "_StateCanOccur", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( Verse_BrokenStateWorker_Binging_StateCanOccur, CCL_BrokenStateWorker_Binging_StateCanOccur );

            // Detour RimWorld.JoyGiver_SocialRelax.TryGiveJob
            MethodInfo RimWorld_JoyGiver_SocialRelax_TryGiveJob = typeof( JoyGiver_SocialRelax ).GetMethod( "TryGiveJob", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_JoyGiver_SocialRelax_TryGiveJob = typeof( Detour._JoyGiver_SocialRelax ).GetMethod( "_TryGiveJob", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_JoyGiver_SocialRelax_TryGiveJob, CCL_JoyGiver_SocialRelax_TryGiveJob );

            // Detour RimWorld.RoomRoleWorker_Laboratory.GetScore
            MethodInfo RimWorld_RoomRoleWorker_Laboratory_GetScore = typeof( RoomRoleWorker_Laboratory ).GetMethod( "GetScore", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_RoomRoleWorker_Laboratory_GetScore = typeof( Detour._RoomRoleWorker_Laboratory ).GetMethod( "_GetScore", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_RoomRoleWorker_Laboratory_GetScore, CCL_RoomRoleWorker_Laboratory_GetScore );

            // Detour RimWorld.WorkGiver_Researcher.HasJobOnThing
            MethodInfo RimWorld_WorkGiver_Researcher_HasJobOnThing = typeof( WorkGiver_Researcher ).GetMethod( "HasJobOnThing", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo CCL_WorkGiver_Researcher_HasJobOnThing = typeof( Detour._WorkGiver_Researcher ).GetMethod( "_HasJobOnThing", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( RimWorld_WorkGiver_Researcher_HasJobOnThing, CCL_WorkGiver_Researcher_HasJobOnThing );

            /*
            // Detour 
            MethodInfo foo = typeof( foo_class ).GetMethod( "foo_method", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo CCL_bar = typeof( Detour._bar ).GetMethod( "_bar_method", BindingFlags.Static | BindingFlags.NonPublic );
            Detours.TryDetourFromTo( foo, CCL_bar );
            */
        }

    }

}
