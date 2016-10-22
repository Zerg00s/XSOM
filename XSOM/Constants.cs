using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XSOM
{
    public class Constants
    {
        public static string TokenRequestNoADFS = @"<s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope'
xmlns:a='http://www.w3.org/2005/08/addressing'
xmlns:u='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
<s:Header>
<a:Action s:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</a:Action>
<a:ReplyTo>
<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>
</a:ReplyTo>
<a:To s:mustUnderstand='1'>https://login.microsoftonline.com/extSTS.srf</a:To>
<o:Security s:mustUnderstand='1'
xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
<o:UsernameToken>
<o:Username>{0}</o:Username>
<o:Password>{1}</o:Password>
</o:UsernameToken>
</o:Security>
</s:Header>
<s:Body>
<t:RequestSecurityToken xmlns:t='http://schemas.xmlsoap.org/ws/2005/02/trust'>
<wsp:AppliesTo xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy'>
<a:EndpointReference>
<a:Address>{2}</a:Address>
</a:EndpointReference>
</wsp:AppliesTo>
<t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>
<t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
<t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType>
</t:RequestSecurityToken>
</s:Body>
</s:Envelope>
";

        public static string TokenRequestADFS = @"<?xml version='1.0' encoding='UTF-8'?>
<s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:saml='urn:oasis:names:tc:SAML:1.0:assertion' xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' xmlns:wsa='http://www.w3.org/2005/08/addressing' xmlns:wssc='http://schemas.xmlsoap.org/ws/2005/02/sc' xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust'>
    <s:Header>
        <wsa:Action s:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</wsa:Action>
        <wsa:To s:mustUnderstand='1'>{0}</wsa:To>
        <wsa:MessageID>{1}</wsa:MessageID>
        <ps:AuthInfo xmlns:ps='http://schemas.microsoft.com/Passport/SoapServices/PPCRL' Id='PPAuthInfo'>
            <ps:HostingApp>Managed IDCRL</ps:HostingApp>
            <ps:BinaryVersion>6</ps:BinaryVersion>
            <ps:UIVersion>1</ps:UIVersion>
            <ps:Cookies></ps:Cookies>
            <ps:RequestParams>AQAAAAIAAABsYwQAAAAxMDMz</ps:RequestParams>
        </ps:AuthInfo>
        <wsse:Security>
            <wsse:UsernameToken wsu:Id='user'>
                <wsse:Username>{2}</wsse:Username>
                <wsse:Password>{3}</wsse:Password>
            </wsse:UsernameToken>
            <wsu:Timestamp Id='Timestamp'>
                <wsu:Created>{4}</wsu:Created>
                <wsu:Expires>{5}</wsu:Expires>
            </wsu:Timestamp>
        </wsse:Security>
    </s:Header>
    <s:Body>
        <wst:RequestSecurityToken Id='RST0'>
            <wst:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</wst:RequestType>
            <wsp:AppliesTo>
                <wsa:EndpointReference>
                    <wsa:Address>urn:federation:MicrosoftOnline</wsa:Address>
                </wsa:EndpointReference>
            </wsp:AppliesTo>
            <wst:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</wst:KeyType>
        </wst:RequestSecurityToken>
    </s:Body>
</s:Envelope>";


        public static string requestTokenAdfsUrl = @"<?xml version='1.0' encoding='UTF-8'?>
<s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope' xmlns:a='http://www.w3.org/2005/08/addressing' xmlns:u='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
	<s:Header>
		<a:Action s:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue</a:Action>
		<a:RelatesTo>{0}</a:RelatesTo>
		<o:Security s:mustUnderstand='1' xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
			<u:Timestamp u:Id='_0'>
				<u:Created>{1}</u:Created>
				<u:Expires>{2}</u:Expires>
			</u:Timestamp>
		</o:Security>
	</s:Header>
	<s:Body>
		<t:RequestSecurityTokenResponse xmlns:t='http://schemas.xmlsoap.org/ws/2005/02/trust'>
			<t:Lifetime>
				<wsu:Created xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>{1}</wsu:Created>
				<wsu:Expires xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>{12}</wsu:Expires>
			</t:Lifetime>
			<wsp:AppliesTo xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy'>
				<wsa:EndpointReference xmlns:wsa='http://www.w3.org/2005/08/addressing'>
					<wsa:Address>urn:federation:MicrosoftOnline</wsa:Address>
				</wsa:EndpointReference>
			</wsp:AppliesTo>
			<t:RequestedSecurityToken>
				<saml:Assertion MajorVersion='1' MinorVersion='1' AssertionID='{7}' Issuer='{11}' IssueInstant='{3}' xmlns:saml='urn:oasis:names:tc:SAML:1.0:assertion'>
					<saml:Conditions NotBefore='{3}' NotOnOrAfter='{4}'>
						<saml:AudienceRestrictionCondition>
							<saml:Audience>urn:federation:MicrosoftOnline</saml:Audience>
						</saml:AudienceRestrictionCondition>
					</saml:Conditions>
					<saml:AttributeStatement>
						<saml:Subject>
							<saml:NameIdentifier Format='urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified'>{5}</saml:NameIdentifier>
							<saml:SubjectConfirmation>
								<saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod>
							</saml:SubjectConfirmation>
						</saml:Subject>
						<saml:Attribute AttributeName='UPN' AttributeNamespace='http://schemas.xmlsoap.org/claims'>
							<saml:AttributeValue>{6}</saml:AttributeValue>
						</saml:Attribute>
						<saml:Attribute AttributeName='ImmutableID' AttributeNamespace='http://schemas.microsoft.com/LiveID/Federation/2008/05'>
							<saml:AttributeValue>{5}</saml:AttributeValue>
						</saml:Attribute>
					</saml:AttributeStatement>
					<saml:AuthenticationStatement AuthenticationMethod='urn:oasis:names:tc:SAML:1.0:am:password' AuthenticationInstant='{3}'>
						<saml:Subject>
							<saml:NameIdentifier Format='urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified'>{5}</saml:NameIdentifier>
							<saml:SubjectConfirmation>
								<saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod>
							</saml:SubjectConfirmation>
						</saml:Subject>
					</saml:AuthenticationStatement>
					<ds:Signature xmlns:ds='http://www.w3.org/2000/09/xmldsig#'>
						<ds:SignedInfo>
							<ds:CanonicalizationMethod Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#'/>
							<ds:SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1'/>
							<ds:Reference URI='#{7}'>
								<ds:Transforms>
									<ds:Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature'/>
									<ds:Transform Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#'/>
								</ds:Transforms>
								<ds:DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1'/>
								<ds:DigestValue>{8}</ds:DigestValue>
							</ds:Reference>
						</ds:SignedInfo>
						<ds:SignatureValue>{9}</ds:SignatureValue>
						<KeyInfo xmlns='http://www.w3.org/2000/09/xmldsig#'>
							<X509Data>
								<X509Certificate>{10}</X509Certificate>
							</X509Data>
						</KeyInfo>
					</ds:Signature>
				</saml:Assertion>
			</t:RequestedSecurityToken>
			<t:RequestedAttachedReference>
				<o:SecurityTokenReference xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
					<o:KeyIdentifier ValueType='http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.0#SAMLAssertionID'>{7}</o:KeyIdentifier>
				</o:SecurityTokenReference>
			</t:RequestedAttachedReference>
			<t:RequestedUnattachedReference>
				<o:SecurityTokenReference xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
					<o:KeyIdentifier ValueType='http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.0#SAMLAssertionID'>{7}</o:KeyIdentifier>
				</o:SecurityTokenReference>
			</t:RequestedUnattachedReference>
			<t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType>
			<t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
			<t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>
		</t:RequestSecurityTokenResponse>
	</s:Body>
</s:Envelope>";

        public static string geg = @"<?xml version='1.0' encoding='UTF-8'?>
<S:Envelope xmlns:S='http://www.w3.org/2003/05/soap-envelope' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' xmlns:wsa='http://www.w3.org/2005/08/addressing' xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust'>
  <S:Header>
    <wsa:Action S:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</wsa:Action>
    <wsa:To S:mustUnderstand='1'>https://login.microsoftonline.com/rst2.srf</wsa:To>
    <ps:AuthInfo xmlns:ps='http://schemas.microsoft.com/LiveID/SoapServices/v1' Id='PPAuthInfo'>
      <ps:BinaryVersion>5</ps:BinaryVersion>
      <ps:HostingApp>Managed IDCRL</ps:HostingApp>
    </ps:AuthInfo>
    <wsse:Security><saml:Assertion MajorVersion='1' MinorVersion='1' AssertionID='{1}' Issuer='{0}' IssueInstant='2016-04-03T07:33:53.086Z' xmlns:saml='urn:oasis:names:tc:SAML:1.0:assertion'><saml:Conditions NotBefore='2016-04-03T07:33:53.086Z' NotOnOrAfter='2016-04-03T08:33:53.086Z'><saml:AudienceRestrictionCondition><saml:Audience>urn:federation:MicrosoftOnline</saml:Audience></saml:AudienceRestrictionCondition></saml:Conditions><saml:AttributeStatement><saml:Subject><saml:NameIdentifier Format='urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified'>53bPwQ17+E+94TlpTLRRUQ==</saml:NameIdentifier><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject><saml:Attribute AttributeName='UPN' AttributeNamespace='http://schemas.xmlsoap.org/claims'><saml:AttributeValue>{2}</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName='ImmutableID' AttributeNamespace='http://schemas.microsoft.com/LiveID/Federation/2008/05'><saml:AttributeValue>53bPwQ17+E+94TlpTLRRUQ==</saml:AttributeValue></saml:Attribute></saml:AttributeStatement><saml:AuthenticationStatement AuthenticationMethod='urn:oasis:names:tc:SAML:1.0:am:password' AuthenticationInstant='2016-04-03T07:33:53.086Z'><saml:Subject><saml:NameIdentifier Format='urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified'>53bPwQ17+E+94TlpTLRRUQ==</saml:NameIdentifier><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject></saml:AuthenticationStatement><ds:Signature xmlns:ds='http://www.w3.org/2000/09/xmldsig#'><ds:SignedInfo><ds:CanonicalizationMethod Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#' /><ds:SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1' /><ds:Reference URI='#{1}'><ds:Transforms><ds:Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' /><ds:Transform Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#' /></ds:Transforms><ds:DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1' /><ds:DigestValue>{3}</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>{4}</ds:SignatureValue><KeyInfo xmlns='http://www.w3.org/2000/09/xmldsig#'><X509Data><X509Certificate>{5}</X509Certificate></X509Data></KeyInfo></ds:Signature></saml:Assertion></wsse:Security>
  </S:Header>
  <S:Body>
    <wst:RequestSecurityToken xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust' Id='RST0'>
      <wst:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</wst:RequestType>
      <wsp:AppliesTo>
        <wsa:EndpointReference>
          <wsa:Address>sharepoint.com</wsa:Address>
        </wsa:EndpointReference>
      </wsp:AppliesTo>
      <wsp:PolicyReference URI='MBI'></wsp:PolicyReference>
    </wst:RequestSecurityToken>
  </S:Body>
</S:Envelope>";

        public static string StsAuthorityUrl = "https://login.microsoftonline.com/extSTS.srf";
        //public static string StsAuthorityUrl = "https://adfs.jolera.com";

        public static string CookiesUrl = "/_forms/default.aspx?wa=wsignin1.0";
    }
}