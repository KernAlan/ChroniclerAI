   M S B A M L   `   `   `  ���� G  CChroniclerAI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ChroniclerAIChroniclerAI      ChroniclerAI.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:ChroniclerAI       � Title$  ChroniclerAI��$	��768��$
��1024�� � Icon$ /mainicon.png��.��� ����  �� ImageSource$ /bg3.jpg�� j� Stretch$ UniformToFill=� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto � � ColumnDefinitions �� $��* ��� $
��Auto �.+��� $��Chronicler AI v2.2��$��0��$��0��$��	10,10,0,0q�$��14(��� $��1��$��0��$��	10,0,0,20q�.+��� $��Visible=�$��75,10,50,20q�$��Stretch=�$	��500��$	��600��$	��597��.���� -    ��Name$ OutputTextBlock��$��1��$��0�� �� TextWrapping$
 Wrap=� �� AcceptsReturn$
 True������ *OutputTextString+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=��� -   $��Clear��$��65���� $��0��$��1��$��3��$��Stretch=�$��Stretch=�$	��150��$��20q�.+��� $��
Audio File��]� .+��� -   $
��Left=�$��50��$��Browse���� $��Stretch=����� *AudioFilePath+$	 TwoWay=�$
 PropertyChanged=� � DataContext ��  �� RelativeSource ��  �� AncestorType" ��� $��API Key��]� .+��� $��Stretch=����� *ApiKey+$	 TwoWay=� ��  �� " ��� -    ��Name$ RecordButton��$��Start Recording��$��
20,20,20,0q��� -   $ TranscribeButton��$��
Transcribe��$��
20,10,20,0q��� $��ChatGPT Actions��$��0, 10, 0, 0q��� -    ��Name$ ModelSelector���� -   $ SummarizeButton��$��	Summarize��$��
20,10,20,0q��� -   $ HighlightButton��$��	Highlight��$��
20,10,20,0q��� -	   $ EnumerateButton��$��	Enumerate��$��
20,10,20,0q��� -
   $ 	AskButton��$	��Ask��$��
20,10,20,0q��� $
��Help��$��0, 10, 0, 0q��� -   $��Readme��$��
20,10,20,0q��� -   $��I need an API Key��$��
20,10,20,0q��� -   $��About��$��
20,10,20,0q��� -   $��Donate��$��
20,10,20,0q�