/*
                        const string type = "global::Microsoft.Xna.Framework.Graphics.Effect";

                        sb.AppendLine($"{indent}        public sealed class Parameters : IShaderParameters");
                        sb.AppendLine($"{indent}        {{");

                        sb.AppendLine();
                        foreach (var sampler in effectData.Samplers)
                        {
                            // hardcoded to just support textures for now :/
                            sb.AppendLine($"{indent}            public global::Microsoft.Xna.Framework.Graphics.Texture2D {sampler.Key} {{ get; set; }}");
                        }
                        sb.AppendLine();

                        sb.AppendLine();
                        foreach (var uniform in effectData.Uniforms)
                        {
                            var uniformType = GetUniformType(uniform.Value);

                            sb.AppendLine($"{indent}            public {uniformType} {uniform.Key} {{ get; set; }}");
                        }
                        sb.AppendLine();

                        sb.AppendLine($"{indent}            public void Apply(global::Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)");
                        sb.AppendLine($"{indent}            {{");
                        foreach (var name in effectData.Samplers.Select(x => x.Key).Concat(effectData.Uniforms.Select(x => x.Key)))
                        {
                            sb.AppendLine($"{indent}                parameters[\"{name}\"]?.SetValue({name});");
                        }

                        // special case for uTime
                        sb.AppendLine($"{indent}                parameters[\"uTime\"]?.SetValue(global::Terraria.Main.GlobalTimeWrappedHourly);");
                        sb.AppendLine($"{indent}            }}");

                        sb.AppendLine($"{indent}        }}");
                        sb.AppendLine();
                        sb.AppendLine($"{indent}        private static readonly Lazy<Asset<{type}>> lazy = new(() => ModContent.Request<{type}>(KEY));");
                        sb.AppendLine($"{indent}        public static Asset<{type}> Asset => lazy.Value;");
                        sb.AppendLine();

                        foreach (var passName in effectData.Passes.Keys)
                        {
                            sb.AppendLine($"{indent}        public static WrapperShaderData<Parameters> Create{passName}()");
                            sb.AppendLine($"{indent}        {{");
                            sb.AppendLine($"{indent}            return new WrapperShaderData<Parameters>(Asset, \"{passName}\");");
                            sb.AppendLine($"{indent}        }}");
                        }
*/

