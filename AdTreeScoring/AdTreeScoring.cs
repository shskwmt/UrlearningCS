﻿using System;
using System.Collections.Generic;
using Datastructures;

namespace Scoring
{
    class ADTreeScoring
    {
        public ADTreeScoring() { }

        public void Execute(string[] args)
        {
            // 引数のチェック
            if (args.Length != 2)
            {
                // エラーメッセージ表示
                Console.WriteLine("[エラー] 引数の数が不正です");
                Environment.Exit(0);
            }

            // オプションのチェック
            // 暫定的に初期値を代入
            int rMin = 5; // The minimum number of records in the AD-tree nodes.
            char delimiter = ',';
            bool hasHeader = true;
            string sf = "BIC";
            int maxParents = 0;
            string constraintsFile = "";

            // csvファイルの読み込み
            RecordFile recordFile = new RecordFile();
            recordFile.ReadRecord(args[0], hasHeader, delimiter);
            //recordFile.Print();

            // BayesianNetworkの初期化
            BayesianNetwork network = new BayesianNetwork(recordFile);

            // AD-Treeの初期化
            ADTree adTree = new ADTree(rMin, network, recordFile);

            // scoring functionの設定
            sf = sf.ToLower();
            if (maxParents > network.Size() || maxParents < 1)
            {
                maxParents = network.Size() - 1;
            }

            if (sf == "bic")
            {
                int maxParentCount = (int)Math.Log(2 * recordFile.Size() / Math.Log(recordFile.Size()));
                if(maxParentCount < maxParents)
                {
                    maxParents = maxParentCount;
                }
            }
            else if (sf == "fnml") { }
            else if (sf == "bdeu") { }
            else
            {
                throw new ArgumentException("Invalid scoring function. Options are: 'BIC', 'fNML' or 'BDeu'.");
            }

            // TODO constraintsの実装
            Constraints constraints = null;
            if (constraintsFile.Length > 0)
            {
                constraints = Constraints.ParseConstraints(constraintsFile, network);
            }

            ScoringFunction scoringFunction = null;

            List<double> ilogi = LogLikelihoodCalculator.GetLogCache(recordFile.Size());
            LogLikelihoodCalculator llc = new LogLikelihoodCalculator(adTree, network, ilogi);

            // TODO regretの実装
            //std::vector<std::vector<float>*>* regret = scoring::getRegretCache(recordFile.size(), network.getMaxCardinality());

            if (sf == "bic")
            {
                scoringFunction = new BICScoringFunction(network, recordFile, llc, constraints);
            }
            else if (sf == "fnml")
            {
                //
            }
            else if (sf == "bdeu")
            {
                //
            }

            // TODO ScoreCalculatorの実装
            // TODO Parallelクラスによる並列処理
        }
    }
}