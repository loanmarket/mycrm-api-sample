// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The WorkbenchLogType. </summary>
    internal readonly partial struct WorkbenchLogType : IEquatable<WorkbenchLogType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="WorkbenchLogType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public WorkbenchLogType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string EmailValue = "Email";
        private const string MailValue = "Mail";
        private const string NotesValue = "Notes";
        private const string RequirementAddValue = "RequirementAdd";
        private const string RequirementCompletedValue = "RequirementCompleted";
        private const string RequirementUntickedValue = "RequirementUnticked";
        private const string RequirementUpdatedValue = "RequirementUpdated";
        private const string RequirementDeletedValue = "RequirementDeleted";
        private const string NextWorkDateValue = "NextWorkDate";

        /// <summary> Email. </summary>
        public static WorkbenchLogType Email { get; } = new WorkbenchLogType(EmailValue);
        /// <summary> Mail. </summary>
        public static WorkbenchLogType Mail { get; } = new WorkbenchLogType(MailValue);
        /// <summary> Notes. </summary>
        public static WorkbenchLogType Notes { get; } = new WorkbenchLogType(NotesValue);
        /// <summary> RequirementAdd. </summary>
        public static WorkbenchLogType RequirementAdd { get; } = new WorkbenchLogType(RequirementAddValue);
        /// <summary> RequirementCompleted. </summary>
        public static WorkbenchLogType RequirementCompleted { get; } = new WorkbenchLogType(RequirementCompletedValue);
        /// <summary> RequirementUnticked. </summary>
        public static WorkbenchLogType RequirementUnticked { get; } = new WorkbenchLogType(RequirementUntickedValue);
        /// <summary> RequirementUpdated. </summary>
        public static WorkbenchLogType RequirementUpdated { get; } = new WorkbenchLogType(RequirementUpdatedValue);
        /// <summary> RequirementDeleted. </summary>
        public static WorkbenchLogType RequirementDeleted { get; } = new WorkbenchLogType(RequirementDeletedValue);
        /// <summary> NextWorkDate. </summary>
        public static WorkbenchLogType NextWorkDate { get; } = new WorkbenchLogType(NextWorkDateValue);
        /// <summary> Determines if two <see cref="WorkbenchLogType"/> values are the same. </summary>
        public static bool operator ==(WorkbenchLogType left, WorkbenchLogType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="WorkbenchLogType"/> values are not the same. </summary>
        public static bool operator !=(WorkbenchLogType left, WorkbenchLogType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="WorkbenchLogType"/>. </summary>
        public static implicit operator WorkbenchLogType(string value) => new WorkbenchLogType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is WorkbenchLogType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(WorkbenchLogType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
