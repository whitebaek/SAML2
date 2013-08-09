using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SAML2.Schema.XmlDSig;
using SAML2.Utils;

namespace SAML2.Schema.Core
{
    /// <summary>
    /// The &lt;Assertion&gt; element is of the AssertionType complex type. This type specifies the basic
    /// information that is common to all assertions,
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.ASSERTION)]
    [XmlRoot(ElementName, Namespace=Saml20Constants.ASSERTION, IsNullable=false)]
    public class Assertion
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ElementName = "Assertion";

        /// <summary>
        /// Initializes a new instance of the <see cref="Assertion"/> class.
        /// </summary>
        public Assertion()
        {
            Version = Saml20Constants.Version;
        }

        /// <summary>
        /// Gets or sets the issue instant.
        /// The time instant of issue in UTC
        /// </summary>
        /// <value>The issue instant.</value>
        [XmlIgnore]
        public DateTime? IssueInstant { get; set; }

        #region Attributes

        /// <summary>
        /// Gets or sets the ID.
        /// The identifier for this assertion. It is of type xs:ID, and MUST follow the requirements specified in
        /// Section 1.3.4 for identifier uniqueness.
        /// </summary>
        /// <value>The ID.</value>
        [XmlAttribute("ID", DataType = "ID")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets a string representation of the issue instant.
        /// </summary>
        /// <value>The issue instant string.</value>
        [XmlAttribute("IssueInstant")]
        public string IssueInstantString
        {
            get { return IssueInstant.HasValue ? Saml20Utils.ToUtcString(IssueInstant.Value) : null; }
            set { IssueInstant = string.IsNullOrEmpty(value) ? (DateTime?) null : Saml20Utils.FromUtcString(value); }
        }

        /// <summary>
        /// Gets or sets the version.
        /// The version of this assertion. The identifier for the version of SAML defined in this specification is "2.0".
        /// </summary>
        /// <value>The version.</value>
        [XmlAttribute]
        public string Version { get; set; }

        #endregion

        #region Elements

        /// <summary>
        /// Gets or sets the advice.
        /// Additional information related to the assertion that assists processing in certain situations but which
        /// MAY be ignored by applications that do not understand the advice or do not wish to make use of it.
        /// </summary>
        /// <value>The advice.</value>
        [XmlElement("Advice")]
        public Advice Advice { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// Conditions that MUST be evaluated when assessing the validity of and/or when using the assertion.
        /// </summary>
        /// <value>The conditions.</value>
        [XmlElement("Conditions")]
        public Conditions Conditions { get; set; }
        
        /// <summary>
        /// Gets or sets the issuer.
        /// The SAML authority that is making the claim(s) in the assertion. The issuer SHOULD be unambiguous
        /// to the intended relying parties.
        /// This specification defines no particular relationship between the entity represented by this element
        /// and the signer of the assertion (if any). Any such requirements imposed
        /// </summary>
        /// <value>The issuer.</value>
        [XmlElement("Issuer")]
        public NameID Issuer { get; set; }

        /// <summary>
        /// Gets or sets the Statements (AttributeStatement, AuthnStatement and AuthzDecisionStatement types) 
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("AttributeStatement", typeof(AttributeStatement))]
        [XmlElement("AuthnStatement", typeof(AuthnStatement))]
        [XmlElement("AuthzDecisionStatement", typeof(AuthzDecisionStatement))]
        [XmlElement("Statement", typeof(StatementAbstract))]
        public StatementAbstract[] Items { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// An XML Signature that protects the integrity of and authenticates the issuer of the assertion
        /// </summary>
        /// <value>The signature.</value>
        [XmlElement("Signature", Namespace=Saml20Constants.XMLDSIG)]
        public Signature Signature { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// The subject of the statement(s) in the assertion
        /// </summary>
        /// <value>The subject.</value>
        [XmlElement("Subject")]
        public Subject Subject { get; set; }

        #endregion

        /// <summary>
        /// Get the AttributeStatement elements of the Assertion.
        /// </summary>
        /// <returns>A list containing the AttributeStatement instances found in the assertion. An empty list if none could be found.</returns>
        public List<AttributeStatement> GetAttributeStatements()
        {
            return GetStatements<AttributeStatement>();
        }

        /// <summary>
        /// Get the AuthnStatement elements of the Assertion.
        /// </summary>
        /// <returns>A list containing the AuthnStatement instances found in the assertion. An empty list if none could be found.</returns>
        public List<AuthnStatement> GetAuthnStatements()
        {
            return GetStatements<AuthnStatement>();
        }

        /// <summary>
        /// Utility method for extracting statements of a particular type from the list of items.
        /// </summary>
        /// <typeparam name="T">The statement type.</typeparam>
        /// <returns>The statements of the specified type.</returns>
        private List<T> GetStatements<T>() where T : StatementAbstract
        {            
            var result = new List<T>(1);
            result.AddRange(Items.OfType<T>());

            return result;
        }
    }
}
