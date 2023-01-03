using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    class EllipticCurveKeyGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private readonly string theAlgorithm;

        private ECDomainParameters theParameters;
        private DerObjectIdentifier publicKeyParamSet;
        private SecureRandom random;

        //** first constructor
        public EllipticCurveKeyGenerator()
            : this("EC")
        {
        }

        //** second constructor
        public EllipticCurveKeyGenerator(
            string choosen_algorithm)
        {
            this.theAlgorithm = choosen_algorithm ?? 
                throw new ArgumentNullException("algorithm");
        }

        public void Init(KeyGenerationParameters theParams)
        {
            if (theParams is ECKeyGenerationParameters elliptic_curve_parameters)
            {
                this.publicKeyParamSet = 
                    elliptic_curve_parameters.PublicKeyParamSet;
                this.theParameters = 
                    elliptic_curve_parameters.DomainParameters;
            }
            else
            {
                DerObjectIdentifier oid;
                switch (theParams.Strength)
                {
                    case 192:
                        oid = X9ObjectIdentifiers.Prime192v1;
                        break;
                    case 224:
                        oid = SecObjectIdentifiers.SecP224r1;
                        break;                    
                    case 128:
                        oid = SecObjectIdentifiers.SecP128r1;
                        break;
                    case 239:
                        oid = X9ObjectIdentifiers.Prime239v1;
                        break;
                    case 256:
                        oid = X9ObjectIdentifiers.Prime256v1;
                        break;
                    case 384:
                        oid = SecObjectIdentifiers.SecP384r1;
                        break;
                    case 521:
                        oid = SecObjectIdentifiers.SecP521r1;
                        break;
                    default:
                        throw new InvalidParameterException("The key size is not " +
                            "defined or it is unknown.");
                }

                X9ECParameters ecps = IdentifyEllipticCurveByObjectIdentifier(oid);

                this.publicKeyParamSet = oid;
                this.theParameters = new ECDomainParameters(
                    ecps.Curve, 
                    ecps.G, 
                    ecps.N, 
                    ecps.H, 
                    ecps.GetSeed());
            }

            this.random = theParams.Random;

            if (this.random == null)
            {
                this.random = new SecureRandom();
            }
        }

        
        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            BigInteger n = theParameters.N;
            BigInteger d;
            int minWeight = n.BitLength >> 2;

            for (; ; )
            {
                d = new BigInteger(n.BitLength, random);

                if (d.CompareTo(BigInteger.Two) < 0 || d.CompareTo(n) >= 0)
                    continue;

                if (WNafUtilities.GetNafWeight(d) < minWeight)
                    continue;

                break;
            }

            ECPoint ellipticCurvePoint = CreateBasePointMultiplier().
                        Multiply(theParameters.G, d);

            if (publicKeyParamSet != null)
            {
                return new AsymmetricCipherKeyPair(
                    new ECPublicKeyParameters(theAlgorithm,
                                              ellipticCurvePoint, 
                                              publicKeyParamSet),
                    new ECPrivateKeyParameters(theAlgorithm, 
                                               d, 
                                               publicKeyParamSet));
            }

            return new AsymmetricCipherKeyPair(
                new ECPublicKeyParameters(theAlgorithm, 
                                           ellipticCurvePoint, 
                                           theParameters),
                new ECPrivateKeyParameters(theAlgorithm, d, theParameters));
        }

        protected virtual ECMultiplier CreateBasePointMultiplier()
        {
            return new FixedPointCombMultiplier();
        }

        internal static X9ECParameters 
            IdentifyEllipticCurveByObjectIdentifier(DerObjectIdentifier 
            object_identifier)
        {
            X9ECParameters x9_elliptic_curve_parameters = CustomNamedCurves.GetByOid(object_identifier);
            if (x9_elliptic_curve_parameters == null)
            {
                x9_elliptic_curve_parameters = ECNamedCurveTable.GetByOid(object_identifier);
            }
            return x9_elliptic_curve_parameters;
        }

        internal static ECPublicKeyParameters GetCorrespondingPublicKey(
            ECPrivateKeyParameters private_key)
        {
            ECDomainParameters ellipticCurve_DomainParameters = private_key.Parameters;
            ECPoint ellipticCurvePoint = 
                    new FixedPointCombMultiplier().
                        Multiply(ellipticCurve_DomainParameters.G, private_key.D);

            if (private_key.PublicKeyParamSet != null)
            {
                return new ECPublicKeyParameters(private_key.AlgorithmName, 
                                                 ellipticCurvePoint, 
                                                 private_key.PublicKeyParamSet);
            }

            return new ECPublicKeyParameters(private_key.AlgorithmName, 
                                             ellipticCurvePoint, 
                                             ellipticCurve_DomainParameters);
        }
    }
}
