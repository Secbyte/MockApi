﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1200: Using directive must appear within a namespace declaration",
    Justification = "Code style preference.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1101:Prefix local calls with this",
    Justification = "Code style preference.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1309:Field names must not begin with underscore",
    Justification = "Code style preference.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1633:File must have header",
    Justification = "File headers not needed.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1652:Enable XML documentation output",
    Justification = "We have no requirement for this right now, so we can enable it when we do.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1128:Put constructor initializers on their own line",
    Justification = "This does not necessarily make it more readable, use your judgement.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1402:File may only contain a single class",
    Justification = "Should be done in most situations, but in files containing entities, we want to have the configuration alongside.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1111:Closing parenthesis must be on line of last parameter",
    Justification = "Code style preference.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.SpacingRules",
    "SA1009:Closing parenthesis must be spaced correctly",
    Justification = "Code style preference.")]
[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Do not directly await a Task",
    Justification = ".NET Core does not have a synchronization context, so this is redundant.")]
[assembly: SuppressMessage(
    "Design",
    "RCS1090:Call 'ConfigureAwait(false)'.",
    Justification = ".NET Core does not have a synchronization context, so this is redundant.")]
[assembly: SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1500:Braces for multi-line statements must not share line",
    Justification = "This forces the while of a do-while to be on a new line.")]
[assembly: SuppressMessage(
    "Minor Code Smell",
    "S1125:Boolean literals should not be redundant",
    Justification = "More explicit boolean comparison preferred.")]
[assembly: SuppressMessage(
    "Simplification",
    "RCS1049:Simplify boolean comparison.",
    Justification = "More explicit boolean comparison preferred.")]
[assembly: SuppressMessage(
    "Design",
    "EPS06",
    Justification = "Nitpicking")]
[assembly: SuppressMessage(
    "Usage",
    "CA2225:Operator overloads have named alternates",
    Justification = "Required solution is not desirable for implicit operators")]
[assembly: SuppressMessage(
    "SecurityCodeScan",
    "SCS0016:Cross-Site Request Forgery (CSRF)",
    Justification = "CSRF Protection not needed for publicly accessible API")]
[assembly: SuppressMessage(
    "Microsoft.Design",
    "CA1028:Enum storage should be Int32",
    Justification = "Useful to use smaller data types for database mappings")]
