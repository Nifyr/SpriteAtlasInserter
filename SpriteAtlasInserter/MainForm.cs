using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Numerics;

namespace SpriteAtlasInserter
{
    public partial class MainForm : Form
    {
        private readonly string outputDir = Environment.CurrentDirectory + "\\Output";
        private readonly string tempDir = Environment.CurrentDirectory + "\\Temp";
        private readonly Random rng;
        private readonly List<AssetsReplacer> ars;
        private string fileName;
        private BundleFileInstance bfi;
        private Size lastSize = new(128, 128);
        public MainForm()
        {
            InitializeComponent();
            rng = new();
            ars = new();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "AssetBundle(*.*)|*.*",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                Close();
                return;
            }
            AssetsManager am = new();
            fileName = Path.GetFileName(ofd.FileName);
            bfi = am.LoadBundleFile(ofd.FileName, false);
            DecompressBundle(bfi);
            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);
            AssetFileInfoEx afie = afi.table.GetAssetsOfType(687078895).First();
            AssetTypeValueField spriteAtlas = am.GetTypeInstance(afi, afie).GetBaseField();
            List<AssetTypeValueField> m_PackedSprites = spriteAtlas["m_PackedSprites"]["Array"].GetChildrenList().ToList();
            List<AssetTypeValueField> m_PackedSpriteNamesToIndex = spriteAtlas["m_PackedSpriteNamesToIndex"]["Array"].GetChildrenList().ToList();
            List<AssetTypeValueField> m_RenderDataMap = spriteAtlas["m_RenderDataMap"]["Array"].GetChildrenList().ToList();
            List<SpriteAtlasEntry> saes = new();
            for (int i = 0; i < m_PackedSprites.Count; i++)
            {
                SpriteAtlasEntry sae = new();
                sae.sprite.fileID = m_PackedSprites[i]["m_FileID"].GetValue().AsInt();
                sae.sprite.pathID = m_PackedSprites[i]["m_PathID"].GetValue().AsInt64();
                sae.name = m_PackedSpriteNamesToIndex[i].GetValue().AsString();
                sae.renderData.first.first = new uint[4];
                sae.renderData.first.first[0] = m_RenderDataMap[i]["first"]["first"]["data[0]"].GetValue().AsUInt();
                sae.renderData.first.first[1] = m_RenderDataMap[i]["first"]["first"]["data[1]"].GetValue().AsUInt();
                sae.renderData.first.first[2] = m_RenderDataMap[i]["first"]["first"]["data[2]"].GetValue().AsUInt();
                sae.renderData.first.first[3] = m_RenderDataMap[i]["first"]["first"]["data[3]"].GetValue().AsUInt();
                sae.renderData.first.second = m_RenderDataMap[i]["first"]["second"].GetValue().AsInt64();
                sae.renderData.second.texture.fileID = m_RenderDataMap[i]["second"]["texture"]["m_FileID"].GetValue().AsInt();
                sae.renderData.second.texture.pathID = m_RenderDataMap[i]["second"]["texture"]["m_PathID"].GetValue().AsInt64();
                sae.renderData.second.alphaTexture.fileID = m_RenderDataMap[i]["second"]["alphaTexture"]["m_FileID"].GetValue().AsInt();
                sae.renderData.second.alphaTexture.pathID = m_RenderDataMap[i]["second"]["alphaTexture"]["m_PathID"].GetValue().AsInt64();
                sae.renderData.second.textureRect.X = m_RenderDataMap[i]["second"]["textureRect"]["x"].GetValue().AsFloat();
                sae.renderData.second.textureRect.Y = m_RenderDataMap[i]["second"]["textureRect"]["y"].GetValue().AsFloat();
                sae.renderData.second.textureRect.Width = m_RenderDataMap[i]["second"]["textureRect"]["width"].GetValue().AsFloat();
                sae.renderData.second.textureRect.Height = m_RenderDataMap[i]["second"]["textureRect"]["height"].GetValue().AsFloat();
                sae.renderData.second.textureRectOffset.X = m_RenderDataMap[i]["second"]["textureRectOffset"]["x"].GetValue().AsFloat();
                sae.renderData.second.textureRectOffset.Y = m_RenderDataMap[i]["second"]["textureRectOffset"]["y"].GetValue().AsFloat();
                sae.renderData.second.atlasRectOffset.X = m_RenderDataMap[i]["second"]["atlasRectOffset"]["x"].GetValue().AsFloat();
                sae.renderData.second.atlasRectOffset.Y = m_RenderDataMap[i]["second"]["atlasRectOffset"]["y"].GetValue().AsFloat();
                sae.renderData.second.uvTransform.X = m_RenderDataMap[i]["second"]["uvTransform"]["x"].GetValue().AsFloat();
                sae.renderData.second.uvTransform.Y = m_RenderDataMap[i]["second"]["uvTransform"]["y"].GetValue().AsFloat();
                sae.renderData.second.uvTransform.Z = m_RenderDataMap[i]["second"]["uvTransform"]["z"].GetValue().AsFloat();
                sae.renderData.second.uvTransform.W = m_RenderDataMap[i]["second"]["uvTransform"]["w"].GetValue().AsFloat();
                sae.renderData.second.downscaleMultiplier = m_RenderDataMap[i]["second"]["downscaleMultiplier"].GetValue().AsFloat();
                sae.renderData.second.settingsRaw = m_RenderDataMap[i]["second"]["settingsRaw"].GetValue().AsUInt();
                saes.Add(sae);
            }
            saes.Sort();
            listBox1.DataSource = saes;
            ActivateControls();
            listBox1.SelectedIndex = 0;
            EntryChanged(null, new());
        }

        private void EntryChanged(object? sender, EventArgs e)
        {
            DeactivateControls();
            RefreshSAEDisplay();
            ActivateControls();
        }

        private void RefreshSAEDisplay()
        {
            SpriteAtlasEntry selectedSAE = (SpriteAtlasEntry)listBox1.SelectedItems[0];
            spriteFileIDNumericUpDown.Value = selectedSAE.sprite.fileID;
            spritePathIDNumericUpDown.Value = selectedSAE.sprite.pathID;
            nameTextBox.Text = selectedSAE.name;
            guidNumericUpDown0.Value = selectedSAE.renderData.first.first[0];
            guidNumericUpDown1.Value = selectedSAE.renderData.first.first[1];
            guidNumericUpDown2.Value = selectedSAE.renderData.first.first[2];
            guidNumericUpDown3.Value = selectedSAE.renderData.first.first[3];
            renderDataKeyNumericUpDown.Value = selectedSAE.renderData.first.second;
            textureFileIDNumericUpDown.Value = selectedSAE.renderData.second.texture.fileID;
            texturePathIDNumericUpDown.Value = selectedSAE.renderData.second.texture.pathID;
            alphaTextureFileIDNumericUpDown.Value = selectedSAE.renderData.second.alphaTexture.fileID;
            alphaTexturePathIDNumericUpDown.Value = selectedSAE.renderData.second.alphaTexture.pathID;
            textureRectXNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRect.X;
            textureRectYNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRect.Y;
            textureRectWidthNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRect.Width;
            textureRectHeightNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRect.Height;
            textureRectOffsetXNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRectOffset.X;
            textureRectOffsetYNumericUpDown.Value = (decimal)selectedSAE.renderData.second.textureRectOffset.Y;
            atlasRectOffsetXNumericUpDown.Value = (decimal)selectedSAE.renderData.second.atlasRectOffset.X;
            atlasRectOffsetYNumericUpDown.Value = (decimal)selectedSAE.renderData.second.atlasRectOffset.Y;
            uvTransformXNumericUpDown.Value = (decimal)selectedSAE.renderData.second.uvTransform.X;
            uvTransformYNumericUpDown.Value = (decimal)selectedSAE.renderData.second.uvTransform.Y;
            uvTransformZNumericUpDown.Value = (decimal)selectedSAE.renderData.second.uvTransform.Z;
            uvTransformWNumericUpDown.Value = (decimal)selectedSAE.renderData.second.uvTransform.W;
            downscaleMultiplierNumericUpDown.Value = (decimal)selectedSAE.renderData.second.downscaleMultiplier;
            settingsRawNumericUpDown.Value = selectedSAE.renderData.second.settingsRaw;
        }

        private void CommitEdit(object? sender, EventArgs e)
        {
            SpriteAtlasEntry selectedSAE = (SpriteAtlasEntry)listBox1.SelectedItems[0];
            selectedSAE.sprite.fileID = (int)spriteFileIDNumericUpDown.Value;
            selectedSAE.sprite.pathID = (long)spritePathIDNumericUpDown.Value;
            selectedSAE.renderData.first.first[0] = (uint)guidNumericUpDown0.Value;
            selectedSAE.renderData.first.first[1] = (uint)guidNumericUpDown1.Value;
            selectedSAE.renderData.first.first[2] = (uint)guidNumericUpDown2.Value;
            selectedSAE.renderData.first.first[3] = (uint)guidNumericUpDown3.Value;
            selectedSAE.renderData.first.second = (long)renderDataKeyNumericUpDown.Value;
            selectedSAE.renderData.second.texture.fileID = (int)textureFileIDNumericUpDown.Value;
            selectedSAE.renderData.second.texture.pathID = (long)texturePathIDNumericUpDown.Value;
            selectedSAE.renderData.second.alphaTexture.fileID = (int)alphaTextureFileIDNumericUpDown.Value;
            selectedSAE.renderData.second.alphaTexture.pathID = (long)alphaTexturePathIDNumericUpDown.Value;
            selectedSAE.renderData.second.textureRect.X = (float)textureRectXNumericUpDown.Value;
            selectedSAE.renderData.second.textureRect.Y = (float)textureRectYNumericUpDown.Value;
            selectedSAE.renderData.second.textureRect.Width = (float)textureRectWidthNumericUpDown.Value;
            selectedSAE.renderData.second.textureRect.Height = (float)textureRectHeightNumericUpDown.Value;
            selectedSAE.renderData.second.textureRectOffset.X = (float)textureRectOffsetXNumericUpDown.Value;
            selectedSAE.renderData.second.textureRectOffset.Y = (float)textureRectOffsetYNumericUpDown.Value;
            selectedSAE.renderData.second.atlasRectOffset.X = (float)atlasRectOffsetXNumericUpDown.Value;
            selectedSAE.renderData.second.atlasRectOffset.Y = (float)atlasRectOffsetYNumericUpDown.Value;
            selectedSAE.renderData.second.uvTransform.X = (float)uvTransformXNumericUpDown.Value;
            selectedSAE.renderData.second.uvTransform.Y = (float)uvTransformYNumericUpDown.Value;
            selectedSAE.renderData.second.uvTransform.Z = (float)uvTransformZNumericUpDown.Value;
            selectedSAE.renderData.second.uvTransform.W = (float)uvTransformWNumericUpDown.Value;
            selectedSAE.renderData.second.downscaleMultiplier = (float)downscaleMultiplierNumericUpDown.Value;
            selectedSAE.renderData.second.settingsRaw = (uint)settingsRawNumericUpDown.Value;
        }

        private void CommitNameEdit(object? sender, EventArgs e)
        {
            SpriteAtlasEntry selectedSAE = (SpriteAtlasEntry)listBox1.SelectedItems[0];
            if (NameExists(nameTextBox.Text)) return;
            selectedSAE.name = nameTextBox.Text;
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            DeactivateControls();
            SpriteAtlasEntry selectedSAE = (SpriteAtlasEntry)listBox1.SelectedItems[0];
            List<SpriteAtlasEntry> saes = (List<SpriteAtlasEntry>)listBox1.DataSource;
            saes.Sort();
            listBox1.DataSource = saes.ToList();
            listBox1.SelectedIndex = saes.IndexOf(selectedSAE);
            ActivateControls();
        }

        private void ActivateControls()
        {
            listBox1.SelectedIndexChanged += EntryChanged;
            spriteFileIDNumericUpDown.ValueChanged += CommitEdit;
            spritePathIDNumericUpDown.ValueChanged += CommitEdit;
            nameTextBox.TextChanged += CommitNameEdit;
            guidNumericUpDown0.ValueChanged += CommitEdit;
            guidNumericUpDown1.ValueChanged += CommitEdit;
            guidNumericUpDown2.ValueChanged += CommitEdit;
            guidNumericUpDown3.ValueChanged += CommitEdit;
            renderDataKeyNumericUpDown.ValueChanged += CommitEdit;
            textureFileIDNumericUpDown.ValueChanged += CommitEdit;
            texturePathIDNumericUpDown.ValueChanged += CommitEdit;
            alphaTextureFileIDNumericUpDown.ValueChanged += CommitEdit;
            alphaTexturePathIDNumericUpDown.ValueChanged += CommitEdit;
            textureRectXNumericUpDown.ValueChanged += CommitEdit;
            textureRectYNumericUpDown.ValueChanged += CommitEdit;
            textureRectWidthNumericUpDown.ValueChanged += CommitEdit;
            textureRectHeightNumericUpDown.ValueChanged += CommitEdit;
            textureRectOffsetXNumericUpDown.ValueChanged += CommitEdit;
            textureRectOffsetYNumericUpDown.ValueChanged += CommitEdit;
            atlasRectOffsetXNumericUpDown.ValueChanged += CommitEdit;
            atlasRectOffsetYNumericUpDown.ValueChanged += CommitEdit;
            uvTransformXNumericUpDown.ValueChanged += CommitEdit;
            uvTransformYNumericUpDown.ValueChanged += CommitEdit;
            uvTransformZNumericUpDown.ValueChanged += CommitEdit;
            uvTransformWNumericUpDown.ValueChanged += CommitEdit;
            downscaleMultiplierNumericUpDown.ValueChanged += CommitEdit;
            settingsRawNumericUpDown.ValueChanged += CommitEdit;
        }

        private void DeactivateControls()
        {
            listBox1.SelectedIndexChanged -= EntryChanged;
            spriteFileIDNumericUpDown.ValueChanged -= CommitEdit;
            spritePathIDNumericUpDown.ValueChanged -= CommitEdit;
            nameTextBox.TextChanged -= CommitNameEdit;
            guidNumericUpDown0.ValueChanged -= CommitEdit;
            guidNumericUpDown1.ValueChanged -= CommitEdit;
            guidNumericUpDown2.ValueChanged -= CommitEdit;
            guidNumericUpDown3.ValueChanged -= CommitEdit;
            renderDataKeyNumericUpDown.ValueChanged -= CommitEdit;
            textureFileIDNumericUpDown.ValueChanged -= CommitEdit;
            texturePathIDNumericUpDown.ValueChanged -= CommitEdit;
            alphaTextureFileIDNumericUpDown.ValueChanged -= CommitEdit;
            alphaTexturePathIDNumericUpDown.ValueChanged -= CommitEdit;
            textureRectXNumericUpDown.ValueChanged -= CommitEdit;
            textureRectYNumericUpDown.ValueChanged -= CommitEdit;
            textureRectWidthNumericUpDown.ValueChanged -= CommitEdit;
            textureRectHeightNumericUpDown.ValueChanged -= CommitEdit;
            textureRectOffsetXNumericUpDown.ValueChanged -= CommitEdit;
            textureRectOffsetYNumericUpDown.ValueChanged -= CommitEdit;
            atlasRectOffsetXNumericUpDown.ValueChanged -= CommitEdit;
            atlasRectOffsetYNumericUpDown.ValueChanged -= CommitEdit;
            uvTransformXNumericUpDown.ValueChanged -= CommitEdit;
            uvTransformYNumericUpDown.ValueChanged -= CommitEdit;
            uvTransformZNumericUpDown.ValueChanged -= CommitEdit;
            uvTransformWNumericUpDown.ValueChanged -= CommitEdit;
            downscaleMultiplierNumericUpDown.ValueChanged -= CommitEdit;
            settingsRawNumericUpDown.ValueChanged -= CommitEdit;
        }

        private class SpriteAtlasEntry : IComparable<SpriteAtlasEntry>
        {
            public AssetPtr sprite;
            public string name;
            public RenderDataMapPair renderData;

            public SpriteAtlasEntry()
            {
                name = "";
            }

            public int CompareTo(SpriteAtlasEntry? other)
            {
                other = other ?? throw new ArgumentNullException(nameof(other));
                return name.CompareTo(other.name);
            }

            public override string ToString()
            {
                return name;
            }
        }

        private struct AssetPtr
        {
            public int fileID;
            public long pathID;
        }

        private struct RenderDataMapPair
        {
            public RenderDataMapKeyPair first;
            public SpriteAtlasData second;
        }

        private struct RenderDataMapKeyPair
        {
            public uint[] first;
            public long second;
        }

        private struct SpriteAtlasData
        {
            public AssetPtr texture;
            public AssetPtr alphaTexture;
            public RectangleF textureRect;
            public Vector2 textureRectOffset;
            public Vector2 atlasRectOffset;
            public Vector4 uvTransform;
            public float downscaleMultiplier;
            public uint settingsRaw;
        }

        private static void DecompressBundle(BundleFileInstance bfi)
        {
            AssetBundleFile abf = bfi.file;

            MemoryStream stream = new();
            abf.Unpack(abf.reader, new AssetsFileWriter(stream));

            stream.Position = 0;

            AssetBundleFile newAbf = new();
            newAbf.Read(new AssetsFileReader(stream), false);

            abf.reader.Close();
            bfi.file = newAbf;
        }

        private bool NameExists(string name) => ((List<SpriteAtlasEntry>)listBox1.DataSource).Any(sae => sae.name == name);

        private long RandomInt64() => rng.NextInt64(long.MinValue, long.MaxValue);

        private uint RandomUInt32() => (uint)rng.NextInt64(uint.MinValue, (long)uint.MaxValue + 1);

        private static void FillTexture2D(AssetTypeValueField atvf, Size spriteSize)
        {
            atvf["m_ForcedFallbackFormat"].GetValue().Set(4);
            atvf["m_Width"].GetValue().Set(spriteSize.Width);
            atvf["m_Height"].GetValue().Set(spriteSize.Height);
            atvf["m_CompleteImageSize"].GetValue().Set(7744);
            atvf["m_TextureFormat"].GetValue().Set(50);
            atvf["m_MipCount"].GetValue().Set(1);
            atvf["m_ImageCount"].GetValue().Set(1);
            atvf["m_TextureDimension"].GetValue().Set(2);
            atvf["m_TextureSettings"]["m_FilterMode"].GetValue().Set(1);
            atvf["m_TextureSettings"]["m_Aniso"].GetValue().Set(1);
            atvf["m_TextureSettings"]["m_WrapU"].GetValue().Set(1);
            atvf["m_TextureSettings"]["m_WrapV"].GetValue().Set(1);
            atvf["m_TextureSettings"]["m_WrapW"].GetValue().Set(1);
            atvf["m_ColorSpace"].GetValue().Set(1);
            atvf["image data"].GetValue().type = EnumValueTypes.ByteArray;
            atvf["image data"].templateField.valueType = EnumValueTypes.ByteArray;
            AssetTypeByteArray atba = new()
            {
                size = (uint)ImageData.data.Length,
                data = ImageData.data
            };
            atvf["image data"].GetValue().Set(atba);
            atvf["m_StreamData"]["path"].GetValue().Set("");
        }

        private void InsertDummy(object sender, EventArgs e)
        {
            TextInputForm tif = new("Sprite Name", "Input the name of the new sprite:", "NEW_SPRITE", lastSize);
            tif.ShowDialog();
            while (NameExists(tif.OutString))
            {
                if (MessageBox.Show("This name already exists!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    return;
                tif.ShowDialog();
            }
            Size spriteSize = tif.OutSize;
            lastSize = spriteSize;

            AssetsManager am = new();
            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);
            TypeTree tt = afi.file.typeTree;
            Type_0D t0D = AssetHelper.FindTypeTreeTypeByID(tt, 28);
            AssetTypeTemplateField attf = new();
            attf.From0D(t0D, 0);
            AssetTypeValueField texture2DATVF = ValueBuilder.DefaultValueFieldFromTemplate(attf);
            FillTexture2D(texture2DATVF, spriteSize);
            texture2DATVF["m_Name"].GetValue().Set(tif.OutString + "_Texture2D");
            long texture2DPathID = RandomInt64();
            ars.Add(new AssetsReplacerFromMemory(0, texture2DPathID, 28, 0xFFFF, texture2DATVF.WriteToByteArray()));

            AssetTypeValueField spriteATVF = afi.table.GetAssetsOfType(213)
                .Select(afie => am.GetTypeInstance(afi, afie).GetBaseField())
                .First();
            spriteATVF["m_Name"].GetValue().Set(tif.OutString);
            spriteATVF["m_Rect"]["x"].GetValue().Set(0);
            spriteATVF["m_Rect"]["y"].GetValue().Set(0);
            spriteATVF["m_Rect"]["width"].GetValue().Set(spriteSize.Width);
            spriteATVF["m_Rect"]["height"].GetValue().Set(spriteSize.Height);
            spriteATVF["m_Offset"]["x"].GetValue().Set(0);
            spriteATVF["m_Offset"]["y"].GetValue().Set(0);
            uint[] renderDataKey = new uint[4];
            for (int i = 0; i < renderDataKey.Length; i++)
                renderDataKey[i] = RandomUInt32();
            spriteATVF["m_RenderDataKey"]["first"]["data[0]"].GetValue().Set(renderDataKey[0]);
            spriteATVF["m_RenderDataKey"]["first"]["data[1]"].GetValue().Set(renderDataKey[1]);
            spriteATVF["m_RenderDataKey"]["first"]["data[2]"].GetValue().Set(renderDataKey[2]);
            spriteATVF["m_RenderDataKey"]["first"]["data[3]"].GetValue().Set(renderDataKey[3]);
            spriteATVF["m_RD"]["textureRect"]["x"].GetValue().Set(0);
            spriteATVF["m_RD"]["textureRect"]["y"].GetValue().Set(0);
            spriteATVF["m_RD"]["textureRect"]["width"].GetValue().Set(spriteSize.Width);
            spriteATVF["m_RD"]["textureRect"]["height"].GetValue().Set(spriteSize.Height);
            spriteATVF["m_RD"]["textureRectOffset"]["x"].GetValue().Set(0);
            spriteATVF["m_RD"]["textureRectOffset"]["y"].GetValue().Set(0);
            long spritePathID = RandomInt64();
            ars.Add(new AssetsReplacerFromMemory(0, spritePathID, 213, 0xFFFF, spriteATVF.WriteToByteArray()));

            SpriteAtlasEntry sae = new();
            sae.sprite.fileID = 0;
            sae.sprite.pathID = spritePathID;
            sae.name = tif.OutString;
            sae.renderData.first.first = new uint[4];
            sae.renderData.first.first = renderDataKey;
            sae.renderData.first.second = 21300000;
            sae.renderData.second.texture.fileID = 0;
            sae.renderData.second.texture.pathID = texture2DPathID;
            sae.renderData.second.alphaTexture.fileID = 0;
            sae.renderData.second.alphaTexture.pathID = 0;
            sae.renderData.second.textureRect.X = 0;
            sae.renderData.second.textureRect.Y = 0;
            sae.renderData.second.textureRect.Width = spriteSize.Width;
            sae.renderData.second.textureRect.Height = spriteSize.Height;
            sae.renderData.second.textureRectOffset.X = 0;
            sae.renderData.second.textureRectOffset.Y = 0;
            sae.renderData.second.atlasRectOffset.X = 0;
            sae.renderData.second.atlasRectOffset.Y = 0;
            sae.renderData.second.uvTransform.X = 100;
            sae.renderData.second.uvTransform.Y = 1000;
            sae.renderData.second.uvTransform.Z = 100;
            sae.renderData.second.uvTransform.W = 1000;
            sae.renderData.second.downscaleMultiplier = 1;
            sae.renderData.second.settingsRaw = 67;
            List<SpriteAtlasEntry> saes = (List<SpriteAtlasEntry>)listBox1.DataSource;
            saes.Add(sae);
            RefreshListBox();
        }

        private void ExportAndExit(object sender, EventArgs e)
        {
            List<SpriteAtlasEntry> saes = (List<SpriteAtlasEntry>)listBox1.DataSource;
            AssetsManager am = new();
            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);
            AssetFileInfoEx afie = afi.table.GetAssetsOfType(687078895).First();
            AssetTypeValueField spriteAtlas = am.GetTypeInstance(afi, afie).GetBaseField();
            AssetTypeTemplateField spriteATTF = spriteAtlas["m_PackedSprites"]["Array"][0].GetTemplateField();
            List<AssetTypeValueField> spriteATVFs = new();
            AssetTypeTemplateField spriteNameATTF = spriteAtlas["m_PackedSpriteNamesToIndex"]["Array"][0].GetTemplateField();
            List<AssetTypeValueField> spriteNameATVFs = new();
            AssetTypeTemplateField renderDataPairATTF = spriteAtlas["m_RenderDataMap"]["Array"][0].GetTemplateField();
            List<AssetTypeValueField> renderDataPairATVFs = new();
            foreach (SpriteAtlasEntry sae in saes)
            {
                AssetTypeValueField atlasSprite = ValueBuilder.DefaultValueFieldFromTemplate(spriteATTF);
                atlasSprite["m_FileID"].GetValue().Set(sae.sprite.fileID);
                atlasSprite["m_PathID"].GetValue().Set(sae.sprite.pathID);
                spriteATVFs.Add(atlasSprite);
                AssetTypeValueField spriteName = ValueBuilder.DefaultValueFieldFromTemplate(spriteNameATTF);
                spriteName.GetValue().Set(sae.name);
                spriteNameATVFs.Add(spriteName);
                AssetTypeValueField renderDataPair = ValueBuilder.DefaultValueFieldFromTemplate(renderDataPairATTF);
                renderDataPair["first"]["first"]["data[0]"].GetValue().Set(sae.renderData.first.first[0]);
                renderDataPair["first"]["first"]["data[1]"].GetValue().Set(sae.renderData.first.first[1]);
                renderDataPair["first"]["first"]["data[2]"].GetValue().Set(sae.renderData.first.first[2]);
                renderDataPair["first"]["first"]["data[3]"].GetValue().Set(sae.renderData.first.first[3]);
                renderDataPair["first"]["second"].GetValue().Set(sae.renderData.first.second);
                renderDataPair["second"]["texture"]["m_FileID"].GetValue().Set(sae.renderData.second.texture.fileID);
                renderDataPair["second"]["texture"]["m_PathID"].GetValue().Set(sae.renderData.second.texture.pathID);
                renderDataPair["second"]["alphaTexture"]["m_FileID"].GetValue().Set(sae.renderData.second.alphaTexture.fileID);
                renderDataPair["second"]["alphaTexture"]["m_PathID"].GetValue().Set(sae.renderData.second.alphaTexture.pathID);
                renderDataPair["second"]["textureRect"]["x"].GetValue().Set(sae.renderData.second.textureRect.X);
                renderDataPair["second"]["textureRect"]["y"].GetValue().Set(sae.renderData.second.textureRect.Y);
                renderDataPair["second"]["textureRect"]["width"].GetValue().Set(sae.renderData.second.textureRect.Width);
                renderDataPair["second"]["textureRect"]["height"].GetValue().Set(sae.renderData.second.textureRect.Height);
                renderDataPair["second"]["textureRectOffset"]["x"].GetValue().Set(sae.renderData.second.textureRectOffset.X);
                renderDataPair["second"]["textureRectOffset"]["y"].GetValue().Set(sae.renderData.second.textureRectOffset.Y);
                renderDataPair["second"]["atlasRectOffset"]["x"].GetValue().Set(sae.renderData.second.atlasRectOffset.X);
                renderDataPair["second"]["atlasRectOffset"]["y"].GetValue().Set(sae.renderData.second.atlasRectOffset.Y);
                renderDataPair["second"]["uvTransform"]["x"].GetValue().Set(sae.renderData.second.uvTransform.X);
                renderDataPair["second"]["uvTransform"]["y"].GetValue().Set(sae.renderData.second.uvTransform.Y);
                renderDataPair["second"]["uvTransform"]["z"].GetValue().Set(sae.renderData.second.uvTransform.Z);
                renderDataPair["second"]["uvTransform"]["w"].GetValue().Set(sae.renderData.second.uvTransform.W);
                renderDataPair["second"]["downscaleMultiplier"].GetValue().Set(sae.renderData.second.downscaleMultiplier);
                renderDataPair["second"]["settingsRaw"].GetValue().Set(sae.renderData.second.settingsRaw);
                renderDataPairATVFs.Add(renderDataPair);
            }
            spriteAtlas["m_PackedSprites"]["Array"].SetChildrenList(spriteATVFs.ToArray());
            spriteAtlas["m_PackedSpriteNamesToIndex"]["Array"].SetChildrenList(spriteNameATVFs.ToArray());
            spriteAtlas["m_RenderDataMap"]["Array"].SetChildrenList(renderDataPairATVFs.ToArray());
            AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, 0xFFFF, spriteAtlas.WriteToByteArray());
            ars.Add(arfm);
            Directory.CreateDirectory(tempDir);
            string fileLocation = tempDir + "\\" + fileName;
            MemoryStream memoryStream = new();
            AssetsFileWriter afw = new(memoryStream);
            afi.file.dependencies.Write(afw);
            afi.file.Write(afw, 0, ars, 0);
            BundleReplacerFromMemory brfm = new(bfi.file.bundleInf6.dirInf[0].name, null, true, memoryStream.ToArray(), -1);
            afw = new(File.OpenWrite(fileLocation));
            bfi.file.Write(afw, new List<BundleReplacer> { brfm });
            afw.Close();
            am = new();
            bfi = am.LoadBundleFile(fileLocation, false);
            Directory.CreateDirectory(outputDir);
            if (File.Exists(outputDir + "\\" + fileName))
                File.Delete(outputDir + "\\" + fileName);
            FileStream stream = File.OpenWrite(outputDir + "\\" + fileName);
            afw = new AssetsFileWriter(stream);
            bfi.file.Pack(bfi.file.reader, afw, AssetBundleCompressionType.LZ4);
            afw.Close();
            bfi.file.Close();
            bfi.BundleStream.Dispose();
            File.Delete(tempDir + "\\" + fileName);
            MessageBox.Show("AssetBundle placed in output folder.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}