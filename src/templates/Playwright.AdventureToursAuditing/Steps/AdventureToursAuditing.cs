﻿using Microsoft.Playwright;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Templates.Playwright.Auth;
using ChristianSchulz.ObjectInspection.Templates.Playwright.Auditing;

using Playwright = Microsoft.Playwright;

namespace ChristianSchulz.ObjectInspection.Templates.Playwright.AdventureToursAuditing.Steps;

public class AdventureToursAuditing : IStep
{
    private readonly AppSettings _appSettings;

    public AdventureToursAuditing(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task ExecuteAsync()
    {
        using var playwright = await Playwright::Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = _appSettings.Client
        });

        var page = await context.NewPageAsync();

        var adventureToursDemoIdentity = _appSettings.DemoIdentity;
        var adventureToursDemoPassword = _appSettings.DemoPassword;

        await page.SignInAsync(
            adventureToursDemoIdentity,
            adventureToursDemoPassword,
            withAutomaticInspectorLogin: true);

        await page.AuditBusinessObjectInspectionAsync("radio-1234", "charged");
        await page.AuditBusinessObjectInspectionAsync("radio-1234", "operational");
        await page.AuditBusinessObjectInspectionAsync("radio-1234", "condition");

        await page.AuditBusinessObjectInspectionAsync("radio-1424", "charged");
        await page.AuditBusinessObjectInspectionAsync("radio-1424", "operational");
        await page.AuditBusinessObjectInspectionAsync("radio-1424", "condition");

        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2234", "charged");
        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2234", "operational");
        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2234", "condition");

        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2424", "charged");
        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2424", "operational");
        await page.AuditBusinessObjectInspectionAsync("gps-tracker-2424", "condition");

        await page.AuditBusinessObjectInspectionAsync("flashlight-134", "charged");
        await page.AuditBusinessObjectInspectionAsync("flashlight-134", "operational");
        await page.AuditBusinessObjectInspectionAsync("flashlight-134", "condition");

        await page.AuditBusinessObjectInspectionAsync("flashlight-142", "charged");
        await page.AuditBusinessObjectInspectionAsync("flashlight-142", "operational");
        await page.AuditBusinessObjectInspectionAsync("flashlight-142", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1234", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1234", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1234", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1424", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1424", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("radio-1424", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2234", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2234", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2234", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2424", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2424", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("gps-tracker-2424", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-134", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-134", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-134", "condition");

        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-142", "charged");
        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-142", "operational");
        await page.AnnotateBusinessObjectInspectionAuditAsync("flashlight-142", "condition");

        await page.SignOutAsync();
    }
}