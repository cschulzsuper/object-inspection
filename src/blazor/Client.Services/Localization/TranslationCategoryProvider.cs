namespace Super.Paula.Client.Localization;

public class TranslationCategoryProvider
{
    public string? Get<T>()
    {
        var typeName = typeof(T).FullName;

        return typeName switch
        {
            "Super.Paula.Client.Components.Administration.DemoNote"
                => "demo-note",

            "Super.Paula.Client.Components.Administration.SignOutButton"
                => "sign-out-button",


            "Super.Paula.Client.Components.Communication.NotificationBadge"
                => "notification-badge",


            "Super.Paula.Client.Components.Forms.AuthorizedButton"
                => "authorized-button",

            "Super.Paula.Client.Components.Forms.CalenderWeekPager"
                => "calender-week-pager",

            "Super.Paula.Client.Components.Forms.InputDayNumber"
                => "input-day-number",

            "Super.Paula.Client.Components.Forms.InputMilliseconds"
                => "input-milliseconds",

            "Super.Paula.Client.Components.Forms.InputWeek"
                => "input-week",


            "Super.Paula.Client.Components.Inventory.AuditingBadge"
                => "auditing-badge",

            "Super.Paula.Client.Components.Inventory.BusinessObjectInspectionResponseToggle"
                => "business-object-inspection-response-toggle",


            "Super.Paula.Client.Pages.ChangeSecret"
                => "change-secret",

            "Super.Paula.Client.Pages.Index"
                => "index",

            "Super.Paula.Client.Pages.MissingTranslations"
                => "missing-translations",

            "Super.Paula.Client.Pages.Register"
                => "register",

            "Super.Paula.Client.Pages.RegisterChiefInspector"
                => "register-chief-inspector",

            "Super.Paula.Client.Pages.RegisterOrganization"
                => "register-organization",

            "Super.Paula.Client.Pages.SignIn"
                => "sign-in",

            "Super.Paula.Client.Pages.SignInInspector"
                => "sign-in-inspector",


            "Super.Paula.Client.Pages.BusinessObjectInspectionAudits.Audit"
                => "business-object-inspection-audits-audit",

            "Super.Paula.Client.Pages.BusinessObjectInspectionAudits.AuditAnnotation"
                => "business-object-inspection-audits-audit-annotation",

            "Super.Paula.Client.Pages.BusinessObjectInspectionAudits.AuditHistory"
                => "business-object-inspection-audits-audit-history",

            "Super.Paula.Client.Pages.BusinessObjectInspectionAudits.AuditHistoryForBusinessObjects"
                => "business-object-inspection-audits-audit-history-for-business-objects",

            "Super.Paula.Client.Pages.BusinessObjectInspectionAudits.Auditing"
                => "business-object-inspection-audits-auditing",


            "Super.Paula.Client.Pages.BusinessObjectInspections.Schedule"
                => "business-object-inspections-schedule",

            "Super.Paula.Client.Pages.BusinessObjectInspections.View"
                => "business-object-inspections-view",


            "Super.Paula.Client.Pages.BusinessObjects.Create"
                => "business-objects-create",

            "Super.Paula.Client.Pages.BusinessObjects.Edit"
                => "business-objects-edit",

            "Super.Paula.Client.Pages.BusinessObjects.View"
                => "business-objects-view",


            "Super.Paula.Client.Pages.Identities.Create"
                => "identities-create",

            "Super.Paula.Client.Pages.Identities.Edit"
                => "identities-edit",

            "Super.Paula.Client.Pages.Identities.View"
                => "identities-view",


            "Super.Paula.Client.Pages.Inspections.Create"
                => "inspections-create",

            "Super.Paula.Client.Pages.Inspections.Edit"
                => "inspections-edit",

            "Super.Paula.Client.Pages.Inspections.View"
                => "inspections-view",


            "Super.Paula.Client.Pages.Inspectors.Create"
                => "inspectors-create",

            "Super.Paula.Client.Pages.Inspectors.Edit"
                => "inspectors-edit",

            "Super.Paula.Client.Pages.Inspectors.View"
                => "inspectors-view",


            "Super.Paula.Client.Pages.Notifications.View"
                => "notifications-view",


            "Super.Paula.Client.Pages.Organizations.Create"
                => "organizations-create",

            "Super.Paula.Client.Pages.Organizations.Edit"
                => "organizations-edit",

            "Super.Paula.Client.Pages.Organizations.View"
                => "organizations-view",

            "Super.Paula.Client.Shared.MainLayout"
                => "main-layout",

            "Super.Paula.Client.Shared.NavMenu"
                => "nav-menu",

            "Super.Paula.Client.App"
                => "app",

            _ => null
        };
    }
}