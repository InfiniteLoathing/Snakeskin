using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal static class DiagnosticDescriptors
    {
        private const string Category = "Templating";
        
        #if DEBUG
        private const DiagnosticSeverity InfoSeverity = DiagnosticSeverity.Warning;
        private const DiagnosticSeverity ErrorSeverity = DiagnosticSeverity.Warning;
        #else
        private const DiagnosticSeverity InfoSeverity = DiagnosticSeverity.Info;
        private const DiagnosticSeverity ErrorSeverity = DiagnosticSeverity.Error;
        #endif

        public static readonly DiagnosticDescriptor ValueReplacement = new DiagnosticDescriptor(
            id: "SNKS000",
            title: "Value Replacement",
            messageFormat: "Value Replacement: {0}",
            category: Category,
            defaultSeverity: InfoSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidDirective = new DiagnosticDescriptor(
            id: "SNKS001",
            title: "Invalid Directive",
            messageFormat: "Invalid directive name: {0}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor UnexpectedToken = new DiagnosticDescriptor(
            id: "SNKS002",
            title: "Unexpected Token",
            messageFormat: "Unexpected token: {0}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ExpectedArguments = new DiagnosticDescriptor(
            id: "SNKS003",
            title: "Expected Arguments",
            messageFormat: "{0} directive requires arguments",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidArgument = new DiagnosticDescriptor(
            id: "SNKS004",
            title: "Invalid Argument",
            messageFormat: "{0} directive does not accept arguments of type {1}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ValueTypeMismatch = new DiagnosticDescriptor(
            id: "SNKS005",
            title: "Value type mismatch",
            messageFormat: "Value \"{0}\" cannot be declared as {1} because it has already been declared with type {2}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ValueParentTypeCollision = new DiagnosticDescriptor(
            id: "SNKS006",
            title: "Value parent type mismatch",
            messageFormat:
            "Value \"{0}\" cannot be accessed as an object because it has already been declared with type {1}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ValuePropertyTypeCollision = new DiagnosticDescriptor(
            id: "SNKS007",
            title: "Value property type mismatch",
            messageFormat:
            "Value property \"{0}.{1}\" cannot be declared as {2} because it has already been declared with type {3}",
            category: Category,
            defaultSeverity: ErrorSeverity,
            isEnabledByDefault: true);

        public static readonly ImmutableArray<DiagnosticDescriptor> All = ImmutableArray.Create(
            ValueReplacement,
            InvalidDirective,
            UnexpectedToken,
            ExpectedArguments,
            InvalidArgument,
            ValueTypeMismatch,
            ValuePropertyTypeCollision);
    }
}