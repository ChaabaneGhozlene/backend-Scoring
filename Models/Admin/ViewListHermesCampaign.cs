using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesCampaign
{
    public string CustomerOid { get; set; } = null!;

    public int CustomerId { get; set; }

    public string Oid { get; set; } = null!;

    public string Did { get; set; } = null!;

    public string? Description { get; set; }

    public int? Type { get; set; }

    public string? Dbname { get; set; }

    public string? TableName { get; set; }

    public int? MaxChannels { get; set; }

    public double? MaxChannelsPerAgent { get; set; }

    public string? VoiceScript { get; set; }

    public string? VoiceScriptName { get; set; }

    public int? QueueId { get; set; }

    public int AvayaVdn { get; set; }

    public int? QueueSecondId { get; set; }

    public int? QueueSecondTps { get; set; }

    public int? AvgDuration { get; set; }

    public string? WelcomeMsg { get; set; }

    public string? WaitTimeMsg { get; set; }

    public string? MenuMsg { get; set; }

    public string? AgentMsg { get; set; }

    public string? WaitMsg { get; set; }

    public int? WaitMsgDuration { get; set; }

    public int? OverflowMode { get; set; }

    public int? OverflowWaitTime { get; set; }

    public int? OverflowWaitLoop { get; set; }

    public int? OverflowChannels { get; set; }

    public string? NoAgentMsg { get; set; }

    public string? ClosingMsg1 { get; set; }

    public string? ClosingMsg2 { get; set; }

    public string? ClosingMsg3 { get; set; }

    public string? ClosingMsg4 { get; set; }

    public string? ClosingMsg5 { get; set; }

    public string? ClosingMsg6 { get; set; }

    public string? ClosingMsg7 { get; set; }

    public string? RouteDest { get; set; }

    public string? RouteMsg { get; set; }

    public string? RouteFailedMsg { get; set; }

    public string? OverflowMsg { get; set; }

    public string? TelInputMsg { get; set; }

    public string? TelCheckMsg { get; set; }

    public string? TelConfirmMsg { get; set; }

    public string? TelByeMsg { get; set; }

    public string? TelErrorMsg { get; set; }

    public string? VoiceInputMsg { get; set; }

    public string? VoiceCheckMsg { get; set; }

    public string? VoiceConfirmMsg { get; set; }

    public string? VoiceByeMsg { get; set; }

    public string? OpeningId { get; set; }

    public string? HolidayId { get; set; }

    public string? DefLanguage { get; set; }

    public string? OutOperator { get; set; }

    public string? OutPrefix { get; set; }

    public int? OutRing { get; set; }

    public double? OutAbandon { get; set; }

    public int? OutMode { get; set; }

    public int? OutWait { get; set; }

    public int? OutRetries { get; set; }

    public int? ScriptMode { get; set; }

    public string? ScriptAddress { get; set; }

    public string? ScriptParams { get; set; }

    public int? Priority { get; set; }

    public string? Profile { get; set; }

    public int? StatusGroup { get; set; }

    public string? ScriptName { get; set; }

    public int? ScriptId { get; set; }

    public int? State { get; set; }

    public string? MailUser { get; set; }

    public string? MailPwd { get; set; }

    public int? Modif { get; set; }

    public string? QuerySchedulerId { get; set; }

    public int? PreviewCallAfter { get; set; }

    public bool? CallClassification { get; set; }

    public string? CallClassificationNumbers { get; set; }

    public bool? CallTransition { get; set; }

    public string? CallTransitionNumbers { get; set; }

    public int? CallNoAnswer { get; set; }

    public string? CallNoAnswerParam { get; set; }

    public int? CallBusy { get; set; }

    public string? CallBusyParam { get; set; }

    public int? CallDisturbed { get; set; }

    public string? CallDisturbedParam { get; set; }

    public int? CallAnsweringMachine { get; set; }

    public string? CallAnsweringMachineParam { get; set; }

    public int? CallAbandon { get; set; }

    public string? CallAbandonParam { get; set; }

    public int? CallMissed { get; set; }

    public string? CallMissedParam { get; set; }

    public int? PhoneDisplay { get; set; }

    public string? PhoneDisplaySpecific { get; set; }

    public string? AcdFax { get; set; }

    public string? Patience { get; set; }

    public string? HoldMsg { get; set; }

    public string? AutoRecord { get; set; }

    public string? AnsweringMsg { get; set; }

    public string? Crmoid { get; set; }

    public int? PlanningId { get; set; }

    public string? TransferMode { get; set; }

    public string? ProfilCampaign { get; set; }

    public int? Virtual { get; set; }

    public int? CallBackDeadLineDays { get; set; }

    public string? CallBackDeadLineDate { get; set; }

    public int? VoiceMailQueue { get; set; }

    public string? MailTemplateOid { get; set; }

    public string? VoicemailConnection { get; set; }

    public string VoicemailTable { get; set; } = null!;

    public int? VirtualCampDuration { get; set; }

    public string? OutExclude { get; set; }
}
