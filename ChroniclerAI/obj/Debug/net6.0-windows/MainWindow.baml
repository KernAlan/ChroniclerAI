   M S B A M L   `   `   `  ����G  CChroniclerAI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ChroniclerAIChroniclerAI  5   	       ChroniclerAI.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:ChroniclerAI     5   	     � Title$  ChroniclerAI��5   	   $	��768��6   $
��1024��6+    � Icon$ /mainicon.png��68   .��5	      � ��5
   
   �� 5       �� ImageSource$ /bg3.jpg��6    j� Stretch$ UniformToFill=�60    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Chronicler AI v2.2��6   $��0��6-   $��0��6:   $��	10,10,0,0q�6J   $��14(�6]   �� 5   
   $��1��6   $��0��6"   $��	10,0,0,20q�62   .+�5      �� $��Visible=�6   $��75,10,50,20q�6A   $��Stretch=�6V   $	��500��6t   $	��600��6�   $	��597��6�   .��5      �� -   5         ��Name$ OutputTextBlock��5      $��1��63   $��0��6@    �� TextWrapping$
 Wrap=�6P    �� AcceptsReturn$
 True��6d   ��6y   �� *OutputTextString+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=�5      �� 5      -   5        $��Clear��5      $��65��6%   60   5      �� 5!   
   $��0��6   $��1��6"   $��3��62   $��Stretch=�6C   $��Stretch=�6_   $	��150��6}   $��20q�6�   .+�5"      �� $��
Audio File��6   ]� 5#      .+�5$      �� -   5        $
��Left=�5$      $��50��6/   $��Browse��6:   6K   �� 5%      $��Stretch=�6   ��68   �� *AudioFilePath+$	 TwoWay=�$
 PropertyChanged=� � DataContext 6�   ��  �� RelativeSource ��  �� AncestorType" �5&      �� 5'      $��API Key��6   ]� 5(      .+�5)      �� $��Stretch=�6�   ��6   �� *ApiKey+$	 TwoWay=� 6?   ��  �� " �6�   5*      �� 5+      -   5         ��Name$ RecordButton��5+      $��Start Recording��6+   $��
20,20,20,0q�6`   �� 5,      -   5        $ TranscribeButton��5,      $��
Transcribe��6/   $��
20,10,20,0q�6W   �� 5-      $��ChatGPT Actions��6   $��0, 10, 0, 0q�6.   �� 5.      -   5         ��Name$ ModelSelector��5.      6.   �� 5/      -   5        $ SummarizeButton��5/      $��	Summarize��6.   $��
20,10,20,0q�6T   �� 50      -   5        $ HighlightButton��50      $��	Highlight��6.   $��
20,10,20,0q�6T   �� 51      -	   5        $ EnumerateButton��51      $��	Enumerate��6.   $��
20,10,20,0q�6T   �� 52      -
   5        $ 	AskButton��52      $	��Ask��6(   $��
20,10,20,0q�6B   �� 53      $
��Help��6   $��0, 10, 0, 0q�6#   �� 54      -   5        $��Readme��54      $��
20,10,20,0q�6;   �� 55      -   5        $��I need an API Key��55      $��
20,10,20,0q�6G   �� 56      -   5        $��About��56      $��
20,10,20,0q�69   �� 57      -   5        $��Donate��57      $��
20,10,20,0q�6;   58      59      5:      