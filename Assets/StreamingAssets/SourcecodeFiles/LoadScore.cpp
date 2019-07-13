#include "TuneSelect.h"
#include "ConfigLoader.h"

static const int MAX_LENGTH_PATH = 261;
static const char *CHAR_EXTENSION_DATA = ".synth";

void TuneSelect::InitializeList()
{

	char currentDir[MAX_LENGTH_PATH];
	memset(currentDir, '\0', sizeof(currentDir));

	GetCurrentDirectory(sizeof(currentDir), currentDir);
	this -> currentDirectory = std::string(currentDir);
	strncat_s(
		currentDir,
		_countof(currentDir),
		"\\Score",
		_countof(currentDir) - GetStringLengthToZero(currentDir) - 1
	);
	
	// �S���ʂ�ǂݍ���
	this -> LoadAllScore(currentDir, _countof(currentDir));

	// �K�v�ȃt�H���g��ǂݍ���
	this -> tuneListItemFont   = CreateFontToHandle(this -> fontname.c_str(), 22, -1, DX_FONTTYPE_ANTIALIASING);
	this -> sortLabelFont      = CreateFontToHandle(this -> fontname.c_str(), 20, -1, DX_FONTTYPE_ANTIALIASING);
	this -> tuneTitleFont      = CreateFontToHandle(this -> fontname.c_str(), 38, -1, DX_FONTTYPE_ANTIALIASING);
	this -> genreFont          = CreateFontToHandle(this -> fontname.c_str(), 20, -1, DX_FONTTYPE_ANTIALIASING);
	this -> artistFont         = CreateFontToHandle(this -> fontname.c_str(), 16, -1, DX_FONTTYPE_ANTIALIASING);
	this -> levelFont          = CreateFontToHandle(this -> fontname.c_str(), 48, -1, DX_FONTTYPE_ANTIALIASING);
	this -> sortHelpFont       = CreateFontToHandle(this -> fontname.c_str(), 12, -1, DX_FONTTYPE_ANTIALIASING);
	this -> bestScoreTitleFont = this -> genreFont;
	this -> bestScoreFont      = this -> artistFont;
	this -> bpmFont            = this -> tuneListItemFont;
	this -> playTimesFont      = this -> artistFont;

	// �\�[�g�A�C�R����ǂݍ���
	this -> sortIcons.push_back(this -> graphics.GetInfo("Level"));
	this -> sortIcons.push_back(this -> graphics.GetInfo("Clear"));
	this -> sortIcons.push_back(this -> graphics.GetInfo("Title"));

	// ��Փx�̉摜��ǂݍ���
	const char *DIF_IMAGE_IDS[5] = {"BEGINNER", "STANDARD", "ADVANCED", "EXTENDED", "LUNATIC"};
	for(int idx = 0; idx < 5; idx++)
	{

		this -> difficulties.push_back(
			this -> graphics.GetInfo(std::string(DIF_IMAGE_IDS[idx]))
		);

	}

	// �\�[�g��ʂ̃��x����p�ӂ���
	this -> sortLabels.push_back("���x����");
	this -> sortLabels.push_back("�N���A��ԏ�");
	this -> sortLabels.push_back("ABC��");

	// �ō��L�^�̃��x���̈ʒu
	const int Y_START_BESTSCORE = 538;
	const int MARGIN_BESTSCORE  = 22;
	for(int idx = 0; idx < 5; idx++)
	{

		Image label;

		label.PositionX(64.0f);
		label.PositionY((float)Y_START_BESTSCORE + (float)(MARGIN_BESTSCORE * idx));

		this -> bestRecords.push_back(label);

	}

	const char *_status[8] = {"��", "���s", "�Ր�", "����", "�", "����", "�S�q", "�^�P"};
	for(int idx = 0; idx < 8; idx++) this -> clearStatus.push_back(std::string(_status[idx]));

	const int colors[8] = {0x444444, 0x7C0707, 0x44C15F, 0xE1781A, 0xFFFFFF, 0xEDEB58, 0xACE5F3, 0xEB133E};
	for(int idx = 0; idx < 8; idx++) this -> clearStatusColors.push_back(colors[idx]);

	this -> selectingListTransparency = 255;

}

void TuneSelect::LoadScore(std::string scoreFilePath)
{

	ScoreLoader score(scoreFilePath.c_str());
	score.Load();

	SynthScore newScore;
	newScore.file   = scoreFilePath;
	newScore.title  = score.GetHeaders("TITLE" ).begin() -> Data();
	newScore.genre  = score.GetHeaders("GENRE" ).begin() -> Data();
	newScore.artist = score.GetHeaders("ARTIST").begin() -> Data();

	std::list<Article> data = score.GetAllArticles();

	this -> FindMaxAndMinBpm(
		score.GetHeaders("BPM"),
		data,
		&newScore.startBpm,
		&newScore.maxBpm,
		&newScore.minBpm
	);

	newScore.notes      = this -> CountTotalNotes(data);
	newScore.level      = (int)strtol(score.GetHeaders("PLAYLEVEL" ).begin() -> Data().c_str(), NULL, 10);
	newScore.difficulty = (int)strtol(score.GetHeaders("DIFFICULTY").begin() -> Data().c_str(), NULL, 10);

	// �L�^�̓ǂݍ���
	ConfigLoader record("Record.ini");
	FileOpenResult ret = record.Load(true);
	if(ret == FileOpenResult::NewFileCreated)
	{
		
		// �t�@�C�������݂��Ȃ��ꍇ�͐V���ɐ��삷��
		record.AddNewLine(";path=score,clear");

		newScore.point       = 0;
		newScore.clearStatus = 0;
		newScore.bp          = 0;
		newScore.times       = 0;
		newScore.maxCombo    = 0;

		record.AddNewKey(newScore.file, "0,0,0,0,0");
		record.Save();

	} else if(ret == FileOpenResult::FileLoaded) {

		if(!record.Exists(newScore.file))
		{

			// �V�����Ȃ̏ꍇ�͋�̋L�^�����
			newScore.point       = 0;
			newScore.clearStatus = 0;
			newScore.bp          = 0;
			newScore.times       = 0;
			newScore.maxCombo    = 0;

			record.AddNewKey(newScore.file, "0,0,0,0,0");
			record.Save();

		} else {

			// ���ɋL�^����Ă���Ȃ̏ꍇ�͂����ǂݍ���
			std::string val = record.GetValue(newScore.file);
			std::string _score;  // �n�C�X�R�A
			std::string _lamp;   // �N���A��
			std::string _bp;     // ���Ղ��~�X������
			std::string _times;  // �v���C��
			std::string _mxCombo;  // �ő�R���{��

			int idx  = 0;
			int type = 0;
			for(idx; idx < val.size(); idx++)
			{

				if(val.at(idx) == ',')
				{

					type++;

				} else {

					switch(type)
					{

						case 0:   _score.push_back(val.at(idx)); break;
						case 1:    _lamp.push_back(val.at(idx)); break;
						case 2:      _bp.push_back(val.at(idx)); break;
						case 3:   _times.push_back(val.at(idx)); break;
						case 4: _mxCombo.push_back(val.at(idx)); break;

					}

				}

			}

			newScore.point       = (int)strtol(  _score.c_str(), NULL, 10);
			newScore.clearStatus = (int)strtol(   _lamp.c_str(), NULL, 10);
			newScore.bp          = (int)strtol(     _bp.c_str(), NULL, 10);
			newScore.times       = (int)strtol(  _times.c_str(), NULL, 10);
			newScore.maxCombo    = (int)strtol(_mxCombo.c_str(), NULL, 10);

		}

	}

	this -> scores.push_back(newScore);

}

void TuneSelect::LoadAllScore(char *subDirectory, int arraySizeCountOf)
{

	WIN32_FIND_DATA data;
	HANDLE file;
	
	char path[MAX_LENGTH_PATH];
	char dir[MAX_LENGTH_PATH];

	memset(path, '\0', sizeof(path));
	memset(dir , '\0', sizeof(dir));

	if(subDirectory[strlen(subDirectory) - 1] != '\\')
	{

		strncat_s(
			subDirectory,
			arraySizeCountOf,
			"\\",
			arraySizeCountOf - GetStringLengthToZero(subDirectory)
		);

	}

	strncpy_s(path, _countof(path), subDirectory, _countof(path) - 1);
	strncat_s(path, _countof(path), "*.*"       , _countof(path) - GetStringLengthToZero(path));

	file = FindFirstFile(path, &data);
	if(file == INVALID_HANDLE_VALUE)
	{

		FindClose(file);
		return;

	}

	// �S�T�u�f�B���N�g���̒T���J�n
	while(FindNextFile(file, &data))
	{

		if((data.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0
			&& strcmp(data.cFileName, "." ) != 0
			&& strcmp(data.cFileName, "..") != 0) {

			strncpy_s(dir, _countof(dir), subDirectory  , arraySizeCountOf);
			strncat_s(dir, _countof(dir), data.cFileName, _countof(dir) - GetStringLengthToZero(dir) - 1);

			this -> LoadAllScore(dir, _countof(dir));

		} else if(strcmp(data.cFileName, "/") != 0 && strcmp(data.cFileName, "..") != 0) {

			// �t�@�C���ł���ꍇ
			char fullpath[MAX_LENGTH_PATH];
			char extension[MAX_LENGTH_PATH];
			memset(fullpath , '\0', sizeof(fullpath) );
			memset(extension, '\0', sizeof(extension));

			strncat_s(fullpath, _countof(fullpath), subDirectory  , _countof(fullpath) - GetStringLengthToZero(fullpath) - 1);
			strncat_s(fullpath, _countof(fullpath), data.cFileName, _countof(fullpath) - GetStringLengthToZero(fullpath) - 1);

			_splitpath_s(fullpath, NULL, 0, NULL, 0, NULL, 0, extension, sizeof(extension));

			if(strcmp(extension, CHAR_EXTENSION_DATA) == 0)
			{

				// ���ʃt�@�C���ł���Α��΃p�X���擾����
				std::string scoreFile(fullpath);

				std::string::size_type currentDirPath(scoreFile.find(this -> currentDirectory));
				if(currentDirPath != std::string::npos)
					scoreFile.replace(currentDirPath, this -> currentDirectory.length(), "");

				std::string::size_type firstSeparator(scoreFile.find("\\"));
				if(firstSeparator != std::string::npos)
					scoreFile.replace(firstSeparator, 1, "");

				std::string::size_type separator(scoreFile.find("\\"));
				while(separator != std::string::npos)
				{

					scoreFile.replace(separator, 1, "/");
					separator = scoreFile.find("\\", separator + 1); 

				}

				// ���ʂ�ǂݍ���
				this -> LoadScore(scoreFile);

			}

		}

	}

}

void TuneSelect::FindMaxAndMinBpm(std::list<Header> header, std::list<Article> article, int *startDest, int* maxDest, int* minDest)
{

	std::list<Header>::iterator headerFinder = header.begin();
	std::list<BpmDefinition> indexedBpm;
	std::list<BpmDefinition> bpmChanges;

	int bpmAtFirst = 0;

	while(headerFinder != header.end())
	{

		if(headerFinder -> Command() == std::string("BPM"))
		{

			// �X�^�[�g����BPM���擾����
			bpmAtFirst = (int)strtol(headerFinder -> Data().c_str(), NULL, 10);
			*startDest = bpmAtFirst;

		} else {

			// �����łȂ����BPM��`���擾
			BpmDefinition def;
			char id[3];
			memset(id, '\0', sizeof(id));

			id[0] = headerFinder -> Command().at(3);
			id[1] = headerFinder -> Command().at(4);

			def.id  = (int)strtol(id, NULL, 10);
			def.bpm = (int)strtol(headerFinder -> Data().c_str(), NULL, 10);

			indexedBpm.push_back(def);

		}

		++headerFinder;

	}

	std::list<Article>::iterator articleFinder = article.begin();
	while(articleFinder != article.end())
	{

		BpmDefinition def;

		if(articleFinder -> Channel() == 8)
		{

			// �C���f�b�N�X�^BPM��`�i256�ȏ�j
			def.id = 0;
			std::list<BpmDefinition>::iterator it = bpmChanges.begin();
			int targetId = GetBase10FromBase36(articleFinder -> Data().c_str());

			while(it != bpmChanges.end())
			{

				if(it -> id == targetId)
				{

					def.bpm = it -> bpm;
					bpmChanges.push_back(def);

				}

				++it;

			}

		} else if(articleFinder -> Channel() == 3) {

			// 255�܂ł�BPM��`
			def.id = 0;

			char data[3];
			int dataLen = articleFinder -> Data().size() / 2;
			memset(data, '\0', sizeof(data));
			for(int idx = 0; idx < dataLen; idx++)
			{

				data[0] = articleFinder -> Data().at(idx * 2);
				data[1] = articleFinder -> Data().at(idx * 2 + 1);

				// ��f�[�^�łȂ����BPM��`�Ƃ���
				if(strcmp(data, "00") != 0)
				{

					int bpm = GetBase10FromBase16(data);
					def.bpm = bpm;
					bpmChanges.push_back(def);

				}

			}

		}

		++articleFinder;

	}

	int fastestIndexed = 0;
	int fastestDirect  = 0;
	int slowestIndexed = 0;
	int slowestDirect  = 0;

	/*=====================================
		�ő�BPM�����߂�
	=====================================*/
	std::list<BpmDefinition>::iterator indexed = indexedBpm.begin();
	while(indexed != indexedBpm.end())
	{

		if(fastestIndexed < indexed -> bpm) fastestIndexed = indexed -> bpm;
		++indexed;

	}

	std::list<BpmDefinition>::iterator direct = bpmChanges.begin();
	while(direct != bpmChanges.end())
	{

		if(fastestDirect < direct -> bpm) fastestDirect = direct -> bpm;
		++direct;

	}

	if(bpmAtFirst >= fastestIndexed && bpmAtFirst >= fastestDirect)
	{

		// �J�n����BPM����ԑ����ꍇ
		*maxDest = bpmAtFirst;

	} else if(fastestIndexed >= fastestDirect) {

		// �C���f�b�N�X�^��`����ԑ����ꍇ
		*maxDest = fastestIndexed;

	} else {

		// ���ړI��`����ԑ����ꍇ
		*maxDest = fastestDirect;

	}

	/*=====================================
		�ŒxBPM�����߂�
	=====================================*/
	slowestIndexed = *maxDest;
	slowestDirect  = *maxDest;

	indexed = indexedBpm.begin();
	while(indexed != indexedBpm.end())
	{

		if(slowestIndexed > indexed -> bpm) slowestIndexed = indexed -> bpm;
		++indexed;

	}

	direct = bpmChanges.begin();
	while(direct != bpmChanges.end())
	{

		if(slowestDirect > direct -> bpm) slowestDirect = direct -> bpm;
		++direct;

	}

	// �ŒxBPM�����߂�
	if(bpmAtFirst <= slowestIndexed && bpmAtFirst <= slowestDirect)
	{

		*minDest = bpmAtFirst;

	} else if(slowestIndexed <= slowestDirect) {

		*minDest = slowestIndexed;

	} else {

		*minDest = slowestDirect;

	}

}

int TuneSelect::CountTotalNotes(std::list<Article> articles)
{

	int laneChannels[7] = {11, 12, 13, 14, 15, 18, 19};
	int notes = 0;
	std::list<Article>::iterator currentLine = articles.begin();

	std::string chstr;
	std::string note;
	int ch;
	int len;

	while(currentLine != articles.end())
	{

		for(int idx = 0; idx < 7; idx++)
		{

			chstr = std::to_string((long)laneChannels[idx]);
			ch = GetBase10FromBase36(chstr.c_str());
			len = currentLine -> Data().size();

			if(currentLine -> Channel() == ch && len % 2 == 0)
			{

				for(int idx = 0; idx < (len / 2); idx++)
				{

					note.clear();
					note.push_back(currentLine -> Data().at(idx * 2));
					note.push_back(currentLine -> Data().at(idx * 2 + 1));

					if(note != "00")
					{

						notes++;

					}

				}

				break;

			}

		}

		++currentLine;

	}

	return notes;

}